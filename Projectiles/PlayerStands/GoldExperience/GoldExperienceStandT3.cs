using JoJoStands.Buffs.Debuffs;
using JoJoStands.Projectiles.Minions;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.GoldExperience
{
    public class GoldExperienceStandT3 : StandClass
    {
        public override int PunchDamage => 65;
        public override int PunchTime => 10;
        public override int HalfStandHeight => 35;
        public override int FistWhoAmI => 2;
        public override int TierNumber => 3;
        public override int StandOffset => 32;
        public override string PunchSoundName => "GER_Muda";
        public override string PoseSoundName => "TheresADreamInMyHeart";
        public override string SpawnSoundName => "Gold Experience";
        public override StandAttackType StandType => StandAttackType.Melee;

        private string[] abilityNames = new string[3] { "Frog", "Tree", "Butterfly" };

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

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                {
                    StayBehind();
                }

                if (Projectile.owner == Main.myPlayer)
                {
                    if (SpecialKeyPressedNoCooldown())
                    {
                        if (!GoldExperienceAbilityWheel.Visible)
                            GoldExperienceAbilityWheel.OpenAbilityWheel(mPlayer, 3);
                        else
                            GoldExperienceAbilityWheel.CloseAbilityWheel();
                    }

                    float mouseDistance = Vector2.Distance(Main.MouseWorld, player.Center);
                    bool mouseOnPlatform = TileID.Sets.Platforms[Main.tile[(int)(Main.MouseWorld.X / 16f), (int)(Main.MouseWorld.Y / 16f)].TileType];
                    if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && mPlayer.chosenAbility == 0)
                    {
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<GEFrog>(), 1, 0f, Projectile.owner, TierNumber, TierNumber - 1f);
                        Main.projectile[proj].netUpdate = true;
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(6));
                    }
                    if (Main.mouseRight && (Collision.SolidCollision(Main.MouseWorld, 1, 1) || mouseOnPlatform) && !Collision.SolidCollision(Main.MouseWorld - new Vector2(0f, 16f), 1, 1) && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && mouseDistance < MaxDistance && mPlayer.chosenAbility == 1)
                    {
                        int yPos = (((int)Main.MouseWorld.Y / 16) - 3) * 16;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.MouseWorld.X, yPos, 0f, 0f, ModContent.ProjectileType<GETree>(), 1, 0f, Projectile.owner, TierNumber);
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(12));
                    }
                    if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && mPlayer.chosenAbility == 2)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<GEButterfly>(), 1, 0f, Projectile.owner);
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(12));
                    }
                }
            }
            if (mPlayer.standAutoMode)
            {
                BasicPunchAI();
            }
        }

        public override bool PreKill(int timeLeft)
        {
            GoldExperienceAbilityWheel.CloseAbilityWheel();
            return true;
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/GoldExperience/GoldExperience_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 12, true);
            }
        }
    }
}