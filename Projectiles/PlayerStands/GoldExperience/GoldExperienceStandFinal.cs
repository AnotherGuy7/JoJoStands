using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.GoldExperience
{
    public class GoldExperienceStandFinal : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 13;
        }

        public override int punchDamage => 98;
        public override int punchTime => 9;
        public override int halfStandHeight => 35;
        public override float fistWhoAmI => 2f;
        public override float tierNumber => 4f;
        public override int drawOffsetRight => -10;
        public override int drawOffsetLeft => -30;
        public override string punchSoundName => "GER_Muda";

        public bool saidAbility = true;
        public int regencounter = 0;
        public int updateTimer = 0;

        public override void AI()
        {
            SelectFrame();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
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
                        modPlayer.GEAbilityNumber += 1;
                        saidAbility = false;
                    }
                    if (modPlayer.GEAbilityNumber >= 4)
                    {
                        modPlayer.GEAbilityNumber = 0;
                    }
                    if (modPlayer.GEAbilityNumber == 0)
                    {
                        if (!saidAbility)
                        {
                            Main.NewText("Ability: Frog");
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
                            Main.NewText("Ability: Butterflies");
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

                    if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")) && modPlayer.GEAbilityNumber == 0)
                    {
                        Main.mouseLeft = false;
                        int proj = Projectile.NewProjectile(projectile.position, Vector2.Zero, mod.ProjectileType("GEFrog"), 1, 0f, Main.myPlayer, tierNumber, tierNumber - 1f);
                        Main.projectile[proj].netUpdate = true;
                        player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(6));
                    }
                    if (Main.mouseRight && Collision.SolidCollision(Main.MouseWorld, 1, 1) && !player.HasBuff(mod.BuffType("AbilityCooldown")) && modPlayer.GEAbilityNumber == 1)
                    {
                        Projectile.NewProjectile(Main.MouseWorld.X, Main.MouseWorld.Y - 65f, 0f, 0f, mod.ProjectileType("GETree"), 1, 0f, Main.myPlayer, tierNumber);
                        player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(12));
                    }
                    if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")) && modPlayer.GEAbilityNumber == 2)
                    {
                        Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("GEButterfly"), 1, 0f, Main.myPlayer);
                        player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(12));
                    }
                    if (Main.mouseRight && player.velocity == Vector2.Zero && modPlayer.GEAbilityNumber == 3)
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
            if (modPlayer.StandAutoMode)
            {
                BasicPunchAI();
            }
        }

        public virtual void SelectFrame()
        {
            Player player = Main.player[projectile.owner];
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                if (projectile.frameCounter >= punchTime - player.GetModPlayer<MyPlayer>().standSpeedBoosts)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 7)
                {
                    projectile.frame = 8;
                }
                if (projectile.frame >= 12)
                {
                    projectile.frame = 8;
                }
            }
            if (normalFrames)
            {
                if (projectile.frameCounter >= 30)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 8)
                {
                    projectile.frame = 0;
                }
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                projectile.frame = 12;
            }
        }
    }
}