using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ControllableNail : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 6;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.penetrate = 3;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        private const float MouseDistanceLimit = 12f;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            if (projectile.owner == Main.myPlayer && projectile.ai[0] == 0f)
            {
                float xDist = (float)Main.mouseX + Main.screenPosition.X - projectile.Center.X;
                float yDist = (float)Main.mouseY + Main.screenPosition.Y - projectile.Center.Y;
                float mouseDistance = (float)Math.Sqrt(xDist * xDist + yDist * yDist);
                projectile.rotation += (float)projectile.direction * 0.8f;
                if (player.gravDir == -1f)
                    yDist = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - projectile.Center.Y;

                if (player.channel)
                {
                    projectile.rotation += (float)projectile.direction * 0.8f;

                    if (mouseDistance > MouseDistanceLimit)
                    {
                        projectile.rotation += (float)projectile.direction * 0.8f;
                        mouseDistance = MouseDistanceLimit / mouseDistance;
                        xDist *= mouseDistance;
                        yDist *= mouseDistance;
                        Point expectedVelocity = new Point((int)(xDist * 1000f), (int)(yDist * 1000f));
                        Point actualVelocity = new Point((int)(projectile.velocity.X * 1000f), (int)(projectile.velocity.Y * 1000f));
                        if (expectedVelocity.X != actualVelocity.X || expectedVelocity.Y != actualVelocity.Y)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.velocity = new Vector2(xDist, yDist);
                    }
                    else
                    {
                        projectile.rotation += (float)projectile.direction * 0.8f;
                        Point expectedVelocity = new Point((int)(xDist * 1000f), (int)(yDist * 1000f));
                        Point actualVelocity = new Point((int)(projectile.velocity.X * 1000f), (int)(projectile.velocity.Y * 1000f));
                        if (expectedVelocity.X != actualVelocity.X || expectedVelocity.Y != actualVelocity.Y)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.velocity = new Vector2(xDist, yDist);
                    }
                }
                else
                {
                    projectile.rotation += (float)projectile.direction * 0.8f;
                    if (projectile.ai[0] == 0f)
                    {
                        projectile.ai[0] = 1f;
                        projectile.rotation += (float)projectile.direction * 0.8f;
                        projectile.netUpdate = true;

                        if (mouseDistance == 0f)
                        {
                            projectile.Center = new Vector2(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2));
                            projectile.rotation += (float)projectile.direction * 0.8f;
                            xDist = projectile.position.X + (float)projectile.width * 0.5f - projectile.Center.X;
                            yDist = projectile.position.Y + (float)projectile.height * 0.5f - projectile.Center.Y;
                            mouseDistance = (float)Math.Sqrt((double)(xDist * xDist + yDist * yDist));
                        }
                        mouseDistance = MouseDistanceLimit / mouseDistance;
                        projectile.velocity = new Vector2(xDist, yDist) * mouseDistance;
                        if (projectile.velocity == Vector2.Zero)
                            projectile.Kill();
                    }
                }
            }
            int dustIndex = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 202, projectile.velocity.X * -0.3f, projectile.velocity.Y * -0.3f);
            Main.dust[dustIndex].noGravity = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
                crit = true;
        }
    }
}