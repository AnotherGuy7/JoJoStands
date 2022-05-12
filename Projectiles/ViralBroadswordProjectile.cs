using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ViralBroadswordProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 14;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.friendly = true;
            //DrawOriginOffsetY = 20;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            //DrawOffsetX = 25 * player.direction;
            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                Projectile.soundDelay = 12;
            }
            /*if (Main.myPlayer == Projectile.owner)
            {
                if (player.channel && !player.noItems && !player.CCed)
                {
                    float distanceAwayFromPlayer = 1f;
                    if (player.inventory[player.selectedItem].shoot == Projectile.type)
                    {
                        distanceAwayFromPlayer = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
                    }
                    Vector2 velocity = Main.MouseWorld - player.RotatedRelativePoint(player.MountedCenter, true);
                    velocity.Normalize();
                    if (velocity.HasNaNs())
                    {
                        velocity = Vector2.UnitX * (float)player.direction;
                    }
                    velocity *= distanceAwayFromPlayer;
                    if (velocity.X != Projectile.velocity.X || velocity.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity = velocity;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            Vector2 vector14 = Projectile.Center + Projectile.velocity * 3f;
            Lighting.AddLight(vector14, 0.8f, 0.8f, 0.8f);
            Projectile.position = player.RotatedRelativePoint(player.Center, true) + new Vector2(-31f, -36f);*/
            Vector2 direction = Vector2.Zero;
            if (Projectile.owner == Main.myPlayer)
            {
                if (player.channel)
                {
                    direction = Main.MouseWorld - player.Center;        //Cause it has to be found client side
                    Vector2 pos = player.Center + (direction.ToRotation().ToRotationVector2() * 40f) + new Vector2(-40f, -40f);
                    if (Projectile.position != pos)
                    {
                        if (Projectile.Center.X > player.Center.X)
                        {
                            Projectile.direction = 1;
                        }
                        else
                        {
                            Projectile.direction = -1;
                        }
                        Projectile.netUpdate = true;
                    }
                    Projectile.position = pos;
                    //Projectile.rotation = direction.ToRotation();
                }
                else
                {
                    Projectile.Kill();
                }
            }
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            //player.itemRotation = direction.ToRotation();
            Projectile.rotation = player.itemRotation = (float)Math.Atan2(direction.Y * Projectile.direction, direction.X * Projectile.direction);

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= 14)
                    Projectile.frame = 0;
            }
        }
    }
}