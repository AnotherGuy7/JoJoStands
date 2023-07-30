using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Networking;
using JoJoStands.NPCs;
using JoJoStands.Projectiles.Minions;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.GoldExperienceRequiem
{
    public class GoldExperienceRequiemStand : StandClass
    {
        public override float MaxDistance => 98f;
        public override int PunchDamage => 138;
        public override int PunchTime => 11;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 3;
        public override int TierNumber => 5;
        public override Vector2 StandOffset => new Vector2(28, 0);
        public override string PunchSoundName => "GER_Muda";
        public override string PoseSoundName => "ThisIsRequiem";
        public override string SpawnSoundName => "Gold Experience Requiem";
        public override StandAttackType StandType => StandAttackType.Melee;

        private int regencounter = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (SpecialKeyPressed() && !player.HasBuff(ModContent.BuffType<BacktoZero>()))
            {
                mPlayer.backToZeroActive = true;
                SyncCall.SyncBackToZero(player.whoAmI, true);
                player.AddBuff(ModContent.BuffType<BacktoZero>(), 20 * 60);
            }
            if (mPlayer.timestopActive)
                return;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !secondaryAbility)
                    {
                        currentAnimationState = AnimationState.Attack;
                        Punch();
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
                if (!attacking)
                    StayBehind();

                if (!attacking && Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseRight && mPlayer.chosenAbility == 0)
                    {
                        secondaryAbility = true;
                        Projectile.netUpdate = true;
                    }

                    float mouseDistance = Vector2.Distance(Main.MouseWorld, player.Center);
                    bool mouseOnPlatform = TileID.Sets.Platforms[Main.tile[(int)(Main.MouseWorld.X / 16f), (int)(Main.MouseWorld.Y / 16f)].TileType];
                    if (Main.mouseRight)
                    {
                        if (mPlayer.chosenAbility == 1 && (Collision.SolidCollision(Main.MouseWorld, 1, 1) || mouseOnPlatform) && !Collision.SolidCollision(Main.MouseWorld - new Vector2(0f, 16f), 1, 1) && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                        {
                            int yPos = (((int)Main.MouseWorld.Y / 16) - 3) * 16;
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.MouseWorld.X, yPos, 0f, 0f, ModContent.ProjectileType<GETree>(), 1, 0f, Projectile.owner, TierNumber);
                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(12));
                        }
                        else if (mPlayer.chosenAbility == 2 && shootCount <= 0 && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && !player.HasBuff(ModContent.BuffType<DeathLoop>()) && mouseDistance < MaxDistance)
                        {
                            bool targetSuccess = false;
                            for (int n = 0; n < Main.maxNPCs; n++)
                            {
                                NPC npc = Main.npc[n];
                                if (npc.active && !npc.townNPC && npc.lifeMax > 5 && Vector2.Distance(Main.MouseWorld, npc.Center) <= npc.Size.Length())
                                {
                                    targetSuccess = true;
                                    player.AddBuff(ModContent.BuffType<DeathLoop>(), 2);
                                    mPlayer.deathLoopTarget = npc.whoAmI;
                                    npc.GetGlobalNPC<JoJoGlobalNPC>().taggedForDeathLoop = 600;
                                    npc.GetGlobalNPC<JoJoGlobalNPC>().deathLoopOwner = player.whoAmI;
                                    SyncCall.SyncDeathLoopInfo(player.whoAmI, npc.whoAmI);
                                    break;
                                }
                            }

                            if (!targetSuccess)
                            {
                                shootCount += 15;
                                Main.NewText("Right-Click the enemy to target");
                            }
                        }
                        else if (mPlayer.chosenAbility == 3 && player.velocity == Vector2.Zero)
                        {
                            regencounter++;
                            if (Main.rand.Next(0, 2 + 1) == 0)
                            {
                                int dustIndex = Dust.NewDust(player.position, player.width, player.height, DustID.IchorTorch, SpeedY: Main.rand.NextFloat(-1.1f, -0.6f + 1f), Scale: Main.rand.NextFloat(1.1f, 2.4f + 1f));
                                Main.dust[dustIndex].noGravity = true;
                            }
                        }
                    }
                    else
                        regencounter = 0;
                    if (regencounter > 80)
                    {
                        int healamount = Main.rand.Next(25, 50);
                        player.statLife += healamount;
                        player.HealEffect(healamount);
                        regencounter = 0;
                    }
                }

                if (SecondSpecialKeyPressed(false))
                {
                    if (!GoldExperienceRequiemAbilityWheel.Visible)
                        GoldExperienceAbilityWheel.OpenAbilityWheel(mPlayer, 5);
                    else
                        GoldExperienceAbilityWheel.CloseAbilityWheel();
                }

                if (secondaryAbility)
                {
                    currentAnimationState = AnimationState.SecondaryAbility;
                    if (Projectile.frame == 8 && shootCount <= 0)
                    {
                        shootCount += 8;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= 16f;
                        int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<GoldExperienceBeam>(), newPunchDamage + 11, 6f, Projectile.owner);
                        Main.projectile[projIndex].netUpdate = true;
                        SoundStyle item41 = SoundID.Item41;
                        item41.Pitch = -0.6f;
                        SoundEngine.PlaySound(item41, player.Center);
                    }
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        public override bool PreKill(int timeLeft)
        {
            GoldExperienceRequiemAbilityWheel.CloseAbilityWheel();
            return true;
        }

        public override void AnimationCompleted(string animationName)
        {
            if (animationName == "Secondary")
            {
                secondaryAbility = false;
                currentAnimationState = AnimationState.Idle;
            }
        }

        public override void SelectAnimation()
        {
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState;
                Projectile.netUpdate = true;
            }

            if (currentAnimationState == AnimationState.Idle)
                PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Attack)
                PlayAnimation("Attack");
            else if (currentAnimationState == AnimationState.SecondaryAbility)
                PlayAnimation("Secondary");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/GoldExperienceRequiem/GER_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 12, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Secondary")
                AnimateStand(animationName, 11, 4, false);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 6, true);
        }
    }
}