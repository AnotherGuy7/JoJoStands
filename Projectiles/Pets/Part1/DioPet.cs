using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Pets.Part1
{
    public class DioPet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            drawOriginOffsetY = -10;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            //MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (!player.HasBuff(mod.BuffType("DioPetBuff")) || player.dead)
            {
                projectile.Kill();
            }

            Vector2 directionToPlayer = player.Center - projectile.Center;
            directionToPlayer.Normalize();
            directionToPlayer *= player.moveSpeed;
            float xDist = Math.Abs(player.position.X - projectile.position.X);
            if (!WorldGen.SolidTile((int)(player.position.X / 16f), (int)(player.position.Y / 16f) + 4))
            {
                projectile.ai[0] = 1f;
            }
            else
            {
                projectile.ai[0] = 0f;
            }

            if (projectile.position.X > player.position.X)
            {
                projectile.direction = -1;
            }
            else
            {
                projectile.direction = 1;
            }
            projectile.spriteDirection = projectile.direction;

            if (projectile.ai[0] == 0f)
            {
                projectile.tileCollide = true;
                if (projectile.velocity.Y < 6f)
                {
                    projectile.velocity.Y += 0.3f;
                }

                if (xDist >= 72f)
                {
                    projectile.velocity.X = directionToPlayer.X * xDist / 14;
                }
                else
                {
                    projectile.velocity.X *= 0.96f;
                }
            }
            float distance = Vector2.Distance(player.Center, projectile.Center);
            if (projectile.ai[0] == 1f)        //Flying
            {
                if (distance >= 48f)
                {
                    if (Math.Abs(player.velocity.X) > 1f || Math.Abs(player.velocity.Y) > 1f)
                    {
                        directionToPlayer *= distance / 16f;
                        projectile.velocity = directionToPlayer;
                    }
                    else
                    {
                        directionToPlayer *= 0.9f * (distance / 60f);
                        projectile.velocity = directionToPlayer;
                    }
                }
            }
            if (distance >= 300f)        //Out of range
            {
                projectile.tileCollide = false;
                directionToPlayer *= distance / 90f;
                projectile.velocity += directionToPlayer;
            }
            AnimatePet();
        }

        private void AnimatePet()
        {
            if (projectile.ai[0] == 0f)
            {
                projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
                if (projectile.frameCounter >= 11)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                    if (projectile.frame >= 5)
                    {
                        projectile.frame = 0;
                    }
                }
            }
            else
            {
                projectile.frameCounter++;
                if (projectile.frameCounter >= 20)
                {
                    projectile.frame++;
                    projectile.frameCounter = 0;
                    if (projectile.frame >= 6)
                    {
                        projectile.frame = 5;
                    }
                }
                if (projectile.frame <= 4)
                {
                    projectile.frame = 5;
                }
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                projectile.frame = 6;
            }
        }
    }
}