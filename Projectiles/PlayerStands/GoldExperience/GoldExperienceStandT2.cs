using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.GoldExperience
{
    public class GoldExperienceStandT2 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 13;
        }

        public override int punchDamage => 41;
        public override int punchTime => 11;
        public override int halfStandHeight => 35;
        public override float fistWhoAmI => 2f;
        public override float tierNumber => 2f;
        public override int standOffset => -30;
        public override string punchSoundName => "GER_Muda";
        public override string poseSoundName => "TheresADreamInMyHeart";
        public override string spawnSoundName => "Gold Experience";
        public override int standType => 1;

        private int updateTimer = 0;
        private string[] abilityNames = new string[2] { "Frog", "Tree" };

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>());
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                Projectile.netUpdate = true;
            }

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
                        if (!GoldExperienceAbilityWheel.visible)
                            GoldExperienceAbilityWheel.OpenAbilityWheel(mPlayer, 2);
                        else
                            GoldExperienceAbilityWheel.CloseAbilityWheel();
                    }

                    if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && mPlayer.chosenAbility == 0)
                    {
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<GEFrog>(), 1, 0f, Projectile.owner, tierNumber, tierNumber - 1f);
                        Main.projectile[proj].netUpdate = true;
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(6));
                    }
                    if (Main.mouseRight && Collision.SolidCollision(Main.MouseWorld, 1, 1) && !Collision.SolidCollision(Main.MouseWorld - new Vector2(0f, 16f), 1, 1) && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && mPlayer.chosenAbility == 1)
                    {
                        int yPos = (((int)Main.MouseWorld.Y / 16) - 3) * 16;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.MouseWorld.X, yPos, 0f, 0f, ModContent.ProjectileType<GETree>(), 1, 0f, Projectile.owner, tierNumber);
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
                normalFrames = false;
                PlayAnimation("Attack>();
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle>();
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>()).poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose>();
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = Mod.GetTexture("Projectiles/PlayerStands/GoldExperience/GoldExperience_" + animationName);

            if (animationName == "Idle>()
            {
                AnimateStand(animationName, 4, 30, true);
            }
            if (animationName == "Attack>()
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Pose>()
            {
                AnimateStand(animationName, 1, 12, true);
            }
        }
    }
}