using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class NailSlasher : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/ControllableNail"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.friendly = true;
        }

        private const float DistanceFromPlayer = 2f * 16f;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            Vector2 direction = Vector2.Zero;
            if (projectile.owner == Main.myPlayer)
            {
                if (Main.mouseRight)
                {
                    direction = Main.MouseWorld - player.Center;        //Cause it has to be found client side
                    direction.Normalize();
                    Vector2 pos = player.Center + (direction * DistanceFromPlayer);
                    if (projectile.Center != pos)
                    {
                        projectile.direction = 1;
                        if (projectile.Center.X < player.Center.X)
                        {
                            projectile.direction = -1;
                        }
                        player.ChangeDir(projectile.direction);
                        projectile.netUpdate = true;
                    }
                    projectile.Center = pos;
                }
                else
                {
                    projectile.Kill();
                }
            }

            projectile.timeLeft = 2;
            projectile.spriteDirection = projectile.direction;

            //player.heldProj = projectile.whoAmI;
            //player.itemTime = 2;
            //player.itemAnimation = 2;
            //player.itemRotation = (float)Math.Atan2(direction.Y * projectile.direction, direction.X * projectile.direction);      //Gives an index OOB error when there's no item
            projectile.rotation += player.direction * 0.8f;

            int dustIndex = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 202, projectile.velocity.X * -0.3f, projectile.velocity.Y * -0.3f);
            Main.dust[dustIndex].noGravity = true;
        }
    }
}