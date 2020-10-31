using JoJoStands.Networking;
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
        public override int halfStandHeight => 34;
        public override float fistWhoAmI => 3f;
        public override float tierNumber => 5f;
        public override int standOffset => 28;
        public override string punchSoundName => "GER_Muda";
        public override string poseSoundName => "ThisIsRequiem";
        public override int standType => 1;

        private bool saidAbility = true;
        private int regencounter = 0;
        private int updateTimer = 0;


        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!modPlayer.StandAutoMode)
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
                    if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")) && modPlayer.GEAbilityNumber == 0)
                    {
                        normalFrames = false;
                        attackFrames = false;
                        secondaryAbilityFrames = true;
                    }
                    if (Main.mouseRight && Collision.SolidCollision(Main.MouseWorld, 1, 1) && !player.HasBuff(mod.BuffType("AbilityCooldown")) && modPlayer.GEAbilityNumber == 1)
                    {
                        Projectile.NewProjectile(Main.MouseWorld.X, Main.MouseWorld.Y - 65f, 0f, 0f, mod.ProjectileType("GETree"), 1, 0f, Main.myPlayer, tierNumber);
                        player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(10));
                    }
                    if (Main.mouseRight && modPlayer.GEAbilityNumber == 2 && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("DeathLoop")))
                    {
                        player.AddBuff(mod.BuffType("DeathLoop"), 1500);
                    }
                    if (Main.mouseRight && player.velocity == Vector2.Zero && modPlayer.GEAbilityNumber == 3)
                    {
                        regencounter++;
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
                    if (Main.mouseRight && modPlayer.GEAbilityNumber == 4 && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("BacktoZero")))
                    {
                        player.AddBuff(mod.BuffType("BacktoZero"), 1200);
                        modPlayer.BackToZero = true;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            ModNetHandler.effectSync.SendBTZ(256, player.whoAmI, true, player.whoAmI);
                        }
                    }
                }

                if (projectile.owner == Main.myPlayer)
                {
                    if (SpecialKeyPressedNoCooldown())
                    {
                        modPlayer.GEAbilityNumber += 1;
                        saidAbility = false;
                    }
                    if (modPlayer.GEAbilityNumber >= 6)
                    {
                        modPlayer.GEAbilityNumber = 0;
                    }
                    if (modPlayer.GEAbilityNumber == 0)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Scorpion Rock");
                            saidAbility = true;
                        }
                    }
                    if (modPlayer.GEAbilityNumber == 1)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Tree");
                            saidAbility = true;
                        }
                    }
                    if (modPlayer.GEAbilityNumber == 2)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Death Loop");
                            saidAbility = true;
                        }
                    }
                    if (modPlayer.GEAbilityNumber == 3)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Limb Recreation");
                            saidAbility = true;
                        }
                    }
                    if (modPlayer.GEAbilityNumber == 4)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Back to Zero");
                            saidAbility = true;
                        }
                    }
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
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("GoldExperienceRock"), newPunchDamage + 11, 6f, Main.myPlayer);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                        player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(3));
                        secondaryAbilityFrames = false;
                    }
                }
            }
            if (modPlayer.StandAutoMode)
            {
                BasicPunchAI();
            }
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
                AnimationStates(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 8, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimationStates(animationName, 17, 11, true);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 6, true);
            }
        }
    }
}