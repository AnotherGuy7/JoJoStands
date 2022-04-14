using JoJoStands.Networking;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.GoldExperienceRequiem
{
    public class GoldExperienceRequiemStand : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 4;
        }

        public override float maxDistance => 98f;
        public override int punchDamage => 138;
        public override int punchTime => 9;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 3f;
        public override float tierNumber => 5f;
        public override int standOffset => 28;
        public override string punchSoundName => "GER_Muda";
        public override string poseSoundName => "ThisIsRequiem";
        public override string spawnSoundName => "Gold Experience";
        public override int standType => 1;

        private int updateTimer = 0;
        private int regencounter = 0;
        private string[] abilityNames = new string[5] { "Scorpion Beam", "Tree", "Death-Loop", "Limb Recreation", "Back to Zero" };


        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !secondaryAbilityFrames)
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
                if (!attackFrames && projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")) && mPlayer.chosenAbility == 0)
                    {
                        normalFrames = false;
                        attackFrames = false;
                        secondaryAbilityFrames = true;
                    }
                    if (Main.mouseRight && Collision.SolidCollision(Main.MouseWorld, 1, 1) && !Collision.SolidCollision(Main.MouseWorld - new Vector2(0f, 16f), 1, 1) && !player.HasBuff(mod.BuffType("AbilityCooldown")) && mPlayer.chosenAbility == 1)
                    {
                        Projectile.NewProjectile(Main.MouseWorld.X, Main.MouseWorld.Y - 16f, 0f, 0f, mod.ProjectileType("GETree"), 1, 0f, projectile.owner, tierNumber);
                        player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(10));
                    }
                    if (Main.mouseRight && mPlayer.chosenAbility == 2 && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("DeathLoop")))
                    {
                        player.AddBuff(mod.BuffType("DeathLoop"), 1500);
                    }
                    if (Main.mouseRight && player.velocity == Vector2.Zero && mPlayer.chosenAbility == 3)
                    {
                        regencounter++;
                        if (Main.rand.Next(0, 2 + 1) == 0)
                        {
                            int dustIndex = Dust.NewDust(player.position, player.width, player.height, 169, SpeedY: Main.rand.NextFloat(-1.1f, -0.6f + 1f), Scale: Main.rand.NextFloat(1.1f, 2.4f + 1f));
                            Main.dust[dustIndex].noGravity = true;
                        }
                    }
                    else
                    {
                        regencounter = 0;
                    }
                    if (regencounter > 80)
                    {
                        int healamount = Main.rand.Next(25, 50);
                        player.statLife += healamount;
                        player.HealEffect(healamount);
                        regencounter = 0;
                    }
                    if (Main.mouseRight && mPlayer.chosenAbility == 4 && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("BacktoZero")))
                    {
                        player.AddBuff(mod.BuffType("BacktoZero"), 1200);
                        mPlayer.backToZeroActive = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            ModNetHandler.effectSync.SendBTZ(256, player.whoAmI, true, player.whoAmI);
                        }
                    }
                }

                if (SpecialKeyPressedNoCooldown())
                {
                    if (!GoldExperienceRequiemAbilityWheel.visible)
                        GoldExperienceAbilityWheel.OpenAbilityWheel(mPlayer, 5);
                    else
                        GoldExperienceAbilityWheel.CloseAbilityWheel();
                }

                if (secondaryAbilityFrames)
                {
                    normalFrames = false;
                    attackFrames = false;
                    projectile.netUpdate = true;
                    if (projectile.frame == 8 && shootCount <= 0)
                    {
                        shootCount += newPunchTime;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 12f;
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("GoldExperienceBeam"), newPunchDamage + 11, 6f, projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                        player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(3));
                        secondaryAbilityFrames = false;
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
            GoldExperienceRequiemAbilityWheel.CloseAbilityWheel();
            return true;
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/GoldExperienceRequiem/GER_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 11, 11, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 6, true);
            }
        }
    }
}