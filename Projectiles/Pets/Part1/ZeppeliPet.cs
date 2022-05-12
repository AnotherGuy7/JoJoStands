using JoJoStands.Buffs.PetBuffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Pets.Part1
{
    public class ZeppeliPet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            DrawOriginOffsetY = -22;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            //MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (!player.HasBuff(ModContent.BuffType<ZeppeliPetBuff>()) || player.dead)
                Projectile.Kill();

            Vector2 directionToPlayer = player.Center - Projectile.Center;
            directionToPlayer.Normalize();
            directionToPlayer *= player.moveSpeed;
            float xDist = Math.Abs(player.position.X - Projectile.position.X);
            if (!WorldGen.SolidTile((int)(player.position.X / 16f), (int)(player.position.Y / 16f) + 4))
            {
                Projectile.ai[0] = 1f;
            }
            else
            {
                Projectile.ai[0] = 0f;
            }

            if (Projectile.position.X > player.position.X)
            {
                Projectile.direction = -1;
            }
            else
            {
                Projectile.direction = 1;
            }
            Projectile.spriteDirection = Projectile.direction;

            if (Projectile.ai[0] == 0f)
            {
                Projectile.tileCollide = true;
                if (Projectile.velocity.Y < 6f)
                {

                    Projectile.velocity.Y += 0.3f;
                }

                if (xDist >= 72f)
                {
                    Projectile.velocity.X = directionToPlayer.X * xDist / 14;
                }
                else
                {
                    Projectile.velocity.X *= 0.96f;
                }
            }
            float distance = Vector2.Distance(player.Center, Projectile.Center);
            if (Projectile.ai[0] == 1f)        //Flying
            {
                if (distance >= 48f)
                {
                    if (Math.Abs(player.velocity.X) > 1f || Math.Abs(player.velocity.Y) > 1f)
                    {
                        directionToPlayer *= distance / 16f;
                        Projectile.velocity = directionToPlayer;
                    }
                    else
                    {
                        directionToPlayer *= 0.9f * (distance / 60f);
                        Projectile.velocity = directionToPlayer;
                    }
                }
            }
            if (distance >= 300f)        //Out of range
            {
                Projectile.tileCollide = false;
                directionToPlayer *= distance / 90f;
                Projectile.velocity += directionToPlayer;
            }
            AnimatePet();
        }

        private void AnimatePet()
        {
            if (Projectile.ai[0] == 0f)
            {
                Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
                if (Projectile.frameCounter >= 11)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                    if (Projectile.frame >= 5)
                    {
                        Projectile.frame = 0;
                    }
                }
            }
            else
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 8)
                {
                    Projectile.frame++;
                    Projectile.frameCounter = 0;
                    if (Projectile.frame >= 7)
                    {
                        Projectile.frame = 5;
                    }
                }
                if (Projectile.frame <= 4)
                {
                    Projectile.frame = 5;
                }
            }
            /*if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>()).poseMode)
            {
                Projectile.frame = 6;
            }*/
        }
    }
}