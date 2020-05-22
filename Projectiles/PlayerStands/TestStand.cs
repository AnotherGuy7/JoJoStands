using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands
{
    public class TestStand : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 6;
        }

        //public override float shootSpeed => 16f;
        //public override float maxDistance { get; } = 98f;
        public override int punchDamage => 70;
        public override int halfStandHeight => 37;
        public override int punchTime => 7;

        public int timestopPoseTimer = 0;

        /*ripple effect info
        private int rippleCount = 3;
        private int rippleSize = 5;
        private int rippleSpeed = 15;
        private float distortStrength = 100f;*/

        public override void AI()
        {
            SelectFrame();
            //rippleEffectTimer--;
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            //Main.NewText("A: " + attackFrames + "; N: " + normalFrames, Color.DarkGreen);
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && player.whoAmI == Main.myPlayer)
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
                if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("TheWorldBuff")))
                {
                    Timestop(30);
                    timestopPoseTimer += 60;
                }
                if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(mod.BuffType("AbilityCooldown")) && player.HasBuff(mod.BuffType("TheWorldBuff")) && timestopPoseTimer <= 0 && player.ownedProjectileCounts[mod.ProjectileType("RoadRoller")] == 0)
                {
                    shootCount += 12;
                    Vector2 shootVel = Main.MouseWorld - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed + 4f;
                    int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("RoadRoller"), 120, 5f, Main.myPlayer);
                    Main.projectile[proj].netUpdate = true;
                    projectile.netUpdate = true;
                }
                if (timestopPoseTimer > 0)
                {
                    timestopPoseTimer--;
                    normalFrames = false;
                    attackFrames = false;
                    projectile.frame = 6;
                    Main.mouseLeft = false;
                    Main.mouseRight = false;
                }
                /*if (rippleEffectTimer <= 0)
                {
                    Filters.Scene["Shockwave"].Deactivate();
                    rippleEffectTimer = 0;
                }*/
            }
            if (modPlayer.StandAutoMode)
            {
                BasicPunchAI();
            }
        }

        public virtual void SelectFrame()
        {
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                if (projectile.frameCounter >= punchTime)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 1)
                {
                    projectile.frame = 2;
                }
                if (projectile.frame >= 6)
                {
                    projectile.frame = 2;
                }
            }
            if (normalFrames)
            {
                if (projectile.frameCounter >= 30)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
        }

        public virtual void PlayAnimation(string animationName)
        {
            standTexture = mod.GetTexture("Projectiles/PlayerStands/TestStand/TestStand_" + animationName);
            if (animationName == "Idle")
            {
                AnimationStates(animationName, 2, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 4, punchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimationStates(animationName, 17, 11, false);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 200, true, true);     //will loop that frame
            }
        }
    }
}