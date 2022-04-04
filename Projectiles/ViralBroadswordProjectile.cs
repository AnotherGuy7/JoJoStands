using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class ViralBroadswordProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 14;
        }

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 60;
            projectile.friendly = true;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.friendly = true;
            //drawOriginOffsetY = 20;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            //drawOffsetX = 25 * player.direction;
            projectile.soundDelay--;
            if (projectile.soundDelay <= 0)
            {
                Main.PlaySound(SoundID.Item1, projectile.Center);
                projectile.soundDelay = 12;
            }
            /*if (Main.myPlayer == projectile.owner)
            {
                if (player.channel && !player.noItems && !player.CCed)
                {
                    float distanceAwayFromPlayer = 1f;
                    if (player.inventory[player.selectedItem].shoot == projectile.type)
                    {
                        distanceAwayFromPlayer = player.inventory[player.selectedItem].shootSpeed * projectile.scale;
                    }
                    Vector2 velocity = Main.MouseWorld - player.RotatedRelativePoint(player.MountedCenter, true);
                    velocity.Normalize();
                    if (velocity.HasNaNs())
                    {
                        velocity = Vector2.UnitX * (float)player.direction;
                    }
                    velocity *= distanceAwayFromPlayer;
                    if (velocity.X != projectile.velocity.X || velocity.Y != projectile.velocity.Y)
                    {
                        projectile.netUpdate = true;
                    }
                    projectile.velocity = velocity;
                }
                else
                {
                    projectile.Kill();
                }
            }
            Vector2 vector14 = projectile.Center + projectile.velocity * 3f;
            Lighting.AddLight(vector14, 0.8f, 0.8f, 0.8f);
            projectile.position = player.RotatedRelativePoint(player.Center, true) + new Vector2(-31f, -36f);*/
            Vector2 direction = Vector2.Zero;
            if (projectile.owner == Main.myPlayer)
            {
                if (player.channel)
                {
                    direction = Main.MouseWorld - player.Center;        //Cause it has to be found client side
                    Vector2 pos = player.Center + (direction.ToRotation().ToRotationVector2() * 40f) + new Vector2(-40f, -40f);
                    if (projectile.position != pos)
                    {
                        if (projectile.Center.X > player.Center.X)
                        {
                            projectile.direction = 1;
                        }
                        else
                        {
                            projectile.direction = -1;
                        }
                        projectile.netUpdate = true;
                    }
                    projectile.position = pos;
                    //projectile.rotation = direction.ToRotation();
                }
                else
                {
                    projectile.Kill();
                }
            }
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            //player.itemRotation = direction.ToRotation();
            projectile.rotation = player.itemRotation = (float)Math.Atan2(direction.Y * projectile.direction, direction.X * projectile.direction);

            projectile.frameCounter++;
            if (projectile.frameCounter >= 2)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
                if (projectile.frame >= 14)
                {
                    projectile.frame = 0;
                }
            }
        }
    }
}