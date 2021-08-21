using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.GoldExperience
{
    public class GoldExperienceStandFinal : StandClass
    {
        public override int punchDamage => 98;
        public override int punchTime => 9;
        public override int halfStandHeight => 35;
        public override float fistWhoAmI => 2f;
        public override float tierNumber => 4f;
        public override int standOffset => -30;
        public override string punchSoundName => "GER_Muda";
        public override string poseSoundName => "TheresADreamInMyHeart";
        public override string spawnSoundName => "Gold Experience";
        public override int standType => 1;

        public bool saidAbility = true;
        public int regencounter = 0;
        public int updateTimer = 0;

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
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
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

                if (projectile.owner == Main.myPlayer)
                {
                    if (SpecialKeyPressedNoCooldown())
                    {
                        mPlayer.GEAbilityNumber += 1;
                        saidAbility = false;
                    }
                    if (mPlayer.GEAbilityNumber >= 4)
                    {
                        mPlayer.GEAbilityNumber = 0;
                    }
                    if (mPlayer.GEAbilityNumber == 0)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Frog");
                            saidAbility = true;
                        }
                    }
                    if (mPlayer.GEAbilityNumber == 1)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Tree");
                            saidAbility = true;
                        }
                    }
                    if (mPlayer.GEAbilityNumber == 2)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Butterflies");
                            saidAbility = true;
                        }
                    }
                    if (mPlayer.GEAbilityNumber == 3)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Limb Recreation");
                            saidAbility = true;
                        }
                    }

                    if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")) && mPlayer.GEAbilityNumber == 0)
                    {
                        int proj = Projectile.NewProjectile(projectile.position, Vector2.Zero, mod.ProjectileType("GEFrog"), 1, 0f, projectile.owner, tierNumber, tierNumber - 1f);
                        Main.projectile[proj].netUpdate = true;
                        player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(6));
                    }
                    if (Main.mouseRight && Collision.SolidCollision(Main.MouseWorld, 1, 1) && !player.HasBuff(mod.BuffType("AbilityCooldown")) && mPlayer.GEAbilityNumber == 1)
                    {
                        Projectile.NewProjectile(Main.MouseWorld.X, Main.MouseWorld.Y - 65f, 0f, 0f, mod.ProjectileType("GETree"), 1, 0f, projectile.owner, tierNumber);
                        player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(12));
                    }
                    if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")) && mPlayer.GEAbilityNumber == 2)
                    {
                        Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("GEButterfly"), 1, 0f, projectile.owner);
                        player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(12));
                    }
                    if (Main.mouseRight && player.velocity == Vector2.Zero && mPlayer.GEAbilityNumber == 3)
                    {
                        regencounter++;
                    }
                    else
                    {
                        regencounter = 0;
                    }
                    if (regencounter >= 120)
                    {
                        int healamount = Main.rand.Next(25, 50);
                        player.statLife += healamount;
                        player.HealEffect(healamount);
                        regencounter = 0;
                    }
                }
            }
            if (mPlayer.standAutoMode)
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
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/GoldExperience/GoldExperience_" + animationName);

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