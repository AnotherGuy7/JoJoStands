using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HamonPunches : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 11;
        }

        public override void SetDefaults()      //look into ProjectileID.595. Done.
        {
            Projectile.width = 68;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.friendly = true;
            DrawOriginOffsetY = 20;
            Projectile.scale = (int)1.5;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 2)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }
            Player player = Main.player[Projectile.owner];
            if (Projectile.direction == 1)
                DrawOffsetX = 25;
            if (Projectile.direction == -1)
                DrawOffsetX = -2;
            if (Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.frame = 0;

            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                Projectile.soundDelay = 12;
            }
            if (Main.myPlayer == Projectile.owner)
            {
                if (player.channel && !player.noItems && !player.CCed)
                {
                    float scaleFactor6 = 1f;
                    if (player.inventory[player.selectedItem].shoot == Projectile.type)
                    {
                        scaleFactor6 = player.inventory[player.selectedItem].shootSpeed * Projectile.scale;
                    }
                    Vector2 vector13 = Main.MouseWorld - player.RotatedRelativePoint(player.MountedCenter, true);
                    vector13.Normalize();
                    if (vector13.HasNaNs())
                    {
                        vector13 = Vector2.UnitX * (float)player.direction;
                    }
                    vector13 *= scaleFactor6;
                    if (vector13.X != Projectile.velocity.X || vector13.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity = vector13;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            Vector2 vector14 = Projectile.Center + Projectile.velocity * 3f;
            Lighting.AddLight(vector14, 0.8f, 0.8f, 0.8f);
            Projectile.position = player.RotatedRelativePoint(player.Center, true) + new Vector2(-31f, -31f);       //I'm adjusting the hitbox cause it's about a block to the right of the player and completely under the player
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2((double)(Projectile.velocity.Y * (float)Projectile.direction), (double)(Projectile.velocity.X * (float)Projectile.direction));
        }
    }
}