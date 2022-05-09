using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles.Pets.Part1
{
    public class SpeedWagonPet : ModProjectile
    {
        private int hatIndex = -1;
        private int hatMovementTimer = 0;
        private Vector2 hatSpawnPos = Vector2.Zero;
        private bool spawnedHat = false;

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
            DrawOriginOffsetY = -18;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            //MyPlayer mPlayer = player.GetModPlayer<MyPlayer>());
            if (!player.HasBuff(ModContent.BuffType<SpeedWagonPetBuff>()) || player.dead)
            {
                Projectile.Kill();
            }

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
                spawnedHat = false;
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
                if (!spawnedHat && hatIndex == -1)
                {
                    hatSpawnPos = Projectile.Center + new Vector2(0f, -16f);
                    hatIndex = Dust.NewDust(hatSpawnPos, Projectile.width, Projectile.height, Mod.DustType("SpeedWagonsHat>());
                    spawnedHat = true;
                }
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

            if (hatIndex != -1)
            {
                spawnedHat = true;
                hatMovementTimer++;
                if (!Main.dust[hatIndex].active)
                {
                    hatIndex = -1;
                    hatMovementTimer = 0;
                    return;
                }

                Dust hat = Main.dust[hatIndex];
                if (hat.alpha == 0)
                {
                    Vector2 rotation = hatSpawnPos - hat.position;
                    hat.rotation = rotation.ToRotation() + MathHelper.ToRadians(90f);
                    hat.position.X = hatSpawnPos.X + (float)Math.Sin(hatMovementTimer / 10) * 8f;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            if (hatIndex != -1)
            {
                Main.dust[hatIndex].active = false;
            }
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
                if (Projectile.frameCounter >= 6)
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