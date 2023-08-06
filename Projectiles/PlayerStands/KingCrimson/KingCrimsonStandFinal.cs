using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.KingCrimson
{
    public class KingCrimsonStandFinal : StandClass
    {
        public override int PunchDamage => 186;
        public override float PunchKnockback => 5f;
        public override int PunchTime => 20;      //KC's punch timings are based on it's frame, so punchTime has to be 3 frames longer than the duration of the frame KC punches in
        public override int HalfStandHeight => 32;
        public override int FistWhoAmI => 6;
        public override int TierNumber => 4;
        public override Vector2 StandOffset => Vector2.Zero;
        public override string PoseSoundName => "AllThatRemainsAreTheResults";
        public override string SpawnSoundName => "King Crimson";
        public override StandAttackType StandType => StandAttackType.Melee;
        public static readonly SoundStyle TimeskipSound = new SoundStyle("JoJoStands/Sounds/GameSounds/TimeSkip");
        public static readonly SoundStyle KingCrimsonSound = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/KingCrimson");

        private int timeskipStartDelay = 0;
        private int blockSearchTimer = 0;
        private int blockTimer = 0;
        private bool preparingTimeskip = false;

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

            if (secondaryAbility)
            {
                GoInFront();
                currentAnimationState = AnimationState.SecondaryAbility;
            }

            if (blockTimer > 0)
                secondaryAbility = true;
            else
                secondaryAbility = false;

            if (SpecialKeyPressed() && !player.HasBuff(ModContent.BuffType<SkippingTime>()) && timeskipStartDelay <= 0 && mPlayer.kingCrimsonBuffIndex == -1)
            {
                if (!JoJoStands.SoundsLoaded || !JoJoStands.SoundsModAbilityVoicelines)
                    timeskipStartDelay = 80;
                else
                {
                    SoundStyle kingCrimsonSound = KingCrimsonSound;
                    kingCrimsonSound.Volume = JoJoStands.ModSoundsVolume;
                    SoundEngine.PlaySound(kingCrimsonSound, Projectile.position);
                    timeskipStartDelay = 0;
                }
                preparingTimeskip = true;
            }
            if (SpecialKeyPressed(false) && mPlayer.kingCrimsonBuffIndex != -1)
            {
                if (player.buffTime[mPlayer.kingCrimsonBuffIndex] > 10)
                {
                    player.buffTime[mPlayer.kingCrimsonBuffIndex] = 10;
                    mPlayer.kingCrimsonBuffIndex = -1;
                }
            }
            if (preparingTimeskip)
            {
                timeskipStartDelay++;
                if (timeskipStartDelay >= 80)
                {
                    shootCount += 15;
                    mPlayer.timeskipActive = true;
                    player.AddBuff(ModContent.BuffType<SkippingTime>(), 10 * 60);
                    SoundEngine.PlaySound(TimeskipSound);
                    SyncCall.SyncTimeskip(player.whoAmI, true);
                    timeskipStartDelay = 0;
                    preparingTimeskip = false;
                    mPlayer.kingCrimsonAbilityCooldownTime = 30;
                }
            }
            if (player.HasBuff(ModContent.BuffType<SkippingTime>()) && player.HasBuff(ModContent.BuffType<ForesightBuff>()))
                mPlayer.kingCrimsonAbilityCooldownTime = 45;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && mPlayer.canStandBasicAttack && !secondaryAbility && !player.HasBuff(ModContent.BuffType<SkippingTime>()))
                    {
                        attacking = true;
                        currentAnimationState = AnimationState.Attack;
                        Vector2 targetPosition = Main.MouseWorld;
                        if (JoJoStands.StandAimAssist)
                        {
                            float lowestDistance = 4f * 16f;
                            for (int n = 0; n < Main.maxNPCs; n++)
                            {
                                NPC npc = Main.npc[n];
                                if (npc.active && npc.CanBeChasedBy(this, false))
                                {
                                    float distance = Vector2.Distance(npc.Center, Main.MouseWorld);
                                    if (distance < lowestDistance && Collision.CanHitLine(Projectile.Center, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) && npc.lifeMax > 5 && !npc.immortal && !npc.hide && !npc.townNPC && !npc.friendly)
                                    {
                                        targetPosition = npc.Center;
                                        lowestDistance = distance;
                                    }
                                }
                            }
                        }

                        float rotaY = targetPosition.Y - Projectile.Center.Y;
                        Projectile.rotation = MathHelper.ToRadians((rotaY * Projectile.spriteDirection) / 6f);
                        Projectile.spriteDirection = Projectile.direction = targetPosition.X > Projectile.Center.X ? 1 : -1;
                        Vector2 velocityAddition = new Vector2(targetPosition.X, targetPosition.Y) - Projectile.position;
                        velocityAddition.Normalize();
                        velocityAddition *= 5f + mPlayer.standTier;
                        float targetDistance = Vector2.Distance(targetPosition, Projectile.Center);
                        if (targetDistance > 12f)
                            Projectile.velocity = player.velocity + velocityAddition;
                        else
                            Projectile.velocity = Vector2.Zero;

                        if (shootCount <= 0 && (Projectile.frame == 0 || Projectile.frame == 4))
                        {
                            shootCount += newPunchTime / 2;
                            Vector2 shootVel = targetPosition - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), newPunchDamage, PunchKnockback, Projectile.owner, FistWhoAmI);
                            Main.projectile[projIndex].netUpdate = true;
                        }
                        LimitDistance();
                        Projectile.netUpdate = true;
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                }
                if (Main.mouseRight && Projectile.owner == Main.myPlayer && !playerHasAbilityCooldown && !player.HasBuff(ModContent.BuffType<SkippingTime>()))
                {
                    blockTimer = 10;
                    currentAnimationState = AnimationState.SecondaryAbility;
                    if (blockSearchTimer > 0)
                    {
                        blockSearchTimer--;
                        return;
                    }

                    int rectWidth = 56;
                    int rectHeight = 64;
                    Rectangle blockRect = new Rectangle((int)Projectile.Center.X - (rectWidth / 2), (int)Projectile.Center.Y - (rectHeight / 2), rectWidth, rectHeight);
                    for (int p = 0; p < Main.maxProjectiles; p++)
                    {
                        Projectile otherProj = Main.projectile[p];
                        if (otherProj.active)
                        {
                            if (blockRect.Intersects(otherProj.Hitbox) && otherProj.type != Projectile.type && !otherProj.friendly)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    Dust.NewDust(player.position, player.width, player.height, DustID.Clentaminator_Red);
                                }

                                otherProj.penetrate -= 1;
                                if (otherProj.penetrate <= 0)
                                    otherProj.Kill();
                                SyncCall.SyncStandEffectInfo(player.whoAmI, otherProj.whoAmI, 6, 1);
                                secondaryAbility = false;

                                Vector2 repositionOffset = new Vector2(5f * 16f * -player.direction, 0f);
                                while (WorldGen.SolidTile((int)(player.Center.X + repositionOffset.X) / 16, (int)(player.Center.Y + repositionOffset.Y) / 16))
                                {
                                    repositionOffset.Y -= 16f;
                                }
                                player.position += repositionOffset;
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(4));
                                SoundEngine.PlaySound(TimeskipSound);
                                for (int i = 0; i < 20; i++)
                                {
                                    Dust.NewDust(player.position, player.width, player.height, DustID.Clentaminator_Red);
                                }
                            }
                        }
                    }
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.lifeMax > 5 && !npc.immortal && !npc.townNPC && !npc.friendly && !npc.hide && npc.Hitbox.Intersects(blockRect))
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                Dust.NewDust(player.position, player.width, player.height, DustID.Clentaminator_Red);
                            }

                            Vector2 repositionPosition = npc.position + new Vector2(3f * 16f * -npc.direction, 0f);
                            while (WorldGen.SolidTile((int)(player.Center.X + repositionPosition.X) / 16, (int)(player.Center.Y + repositionPosition.Y) / 16))
                            {
                                repositionPosition.Y -= 16f;
                            }
                            player.position = repositionPosition;
                            player.ChangeDir(npc.direction);
                            player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(4));
                            int damage = newPunchDamage * 2;
                            NPC.HitInfo hitInfo = new NPC.HitInfo()
                            {
                                Damage = damage,
                                Knockback = PunchKnockback * 2f,
                                HitDirection = npc.direction
                            };
                            npc.StrikeNPC(hitInfo);
                            SyncCall.SyncStandEffectInfo(player.whoAmI, npc.whoAmI, 6, 2, damage, player.direction, PunchKnockback * 1.5f);
                            SoundEngine.PlaySound(TimeskipSound);

                            for (int i = 0; i < 20; i++)
                            {
                                Dust.NewDust(player.position, player.width, player.height, 114);
                            }
                            break;
                        }
                    }
                    blockSearchTimer += 5;
                    Projectile.netUpdate = true;
                }
                else
                {
                    if (blockTimer > 0)
                        blockTimer--;
                }
                if (!attacking && !secondaryAbility)
                {
                    StayBehind();
                }
                if (SecondSpecialKeyPressed() && shootCount <= 0 && !player.HasBuff(ModContent.BuffType<ForesightBuff>()) && !preparingTimeskip && Projectile.owner == Main.myPlayer)
                {
                    mPlayer.epitaphForesightActive = true;
                    mPlayer.kingCrimsonAbilityCooldownTime = 30;
                    SyncCall.SyncForesight(player.whoAmI, true);
                    player.AddBuff(ModContent.BuffType<ForesightBuff>(), 540);
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
            Projectile.shouldFallThrough = true;
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
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
                PlayAnimation("Block");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/KingCrimson/KingCrimson_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 15, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 6, newPunchTime / 2, true);
            else if (animationName == "Block")
                AnimateStand(animationName, 4, 15, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 2, true);
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(blockTimer);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            blockTimer = reader.ReadInt32();
        }
    }
}