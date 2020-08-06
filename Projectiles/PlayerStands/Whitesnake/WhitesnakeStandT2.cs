using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Whitesnake
{
    public class WhitesnakeStandT2 : StandClass
    {
        public override int punchDamage => 38;
        public override int altDamage => 25;
        public override int punchTime => 13;
        public override int halfStandHeight => 44;
        public override float fistWhoAmI => 9f;
        public override int standType => 1;
        public override float maxDistance => 147f;      //1.5x the normal range cause Whitesnake is considered a long-range stand with melee capabilities

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
                if (Main.mouseRight && shootCount <= 0 && projectile.owner == Main.myPlayer)
                {
                    projectile.frame = 0;
                    secondaryAbilityFrames = true;
                }
                if (secondaryAbilityFrames)
                {
                    Main.mouseLeft = false;
                    if (projectile.frame >= 4)
                    {
                        shootCount += 120;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 8f;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("MeltYourHeart"), (int)(altDamage * modPlayer.standDamageBoosts), 2f, Main.myPlayer, projectile.whoAmI);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                        secondaryAbilityFrames = false;
                    }
                }
                if (!attackFrames)
                {
                    if (!secondaryAbilityFrames)
                        StayBehind();
                    else
                        GoInFront();
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
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/Whitesnake/Whitesnake_" + animationName);

            if (animationName == "Idle")
            {
                AnimationStates(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 3, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimationStates(animationName, 5, 10, true);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 10, true);
            }
        }
    }
}