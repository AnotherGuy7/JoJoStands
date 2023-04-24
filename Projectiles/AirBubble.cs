using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class AirBubble : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private int sinTimer = 0;

        public override void AI()
        {
            sinTimer += 4;
            if (sinTimer >= 360)
                sinTimer = 0;

            Projectile.alpha = (int)(255f * Math.Sin(MathHelper.ToRadians(sinTimer)));

            /*if (Projectile.ai[0] == 0f)
            {
                Projectile.alpha += 6;
            }
            if (Projectile.ai[0] == 1f)
            {
                Projectile.alpha -= 6;
            }
            if (Projectile.alpha >= 255)
            {
                Projectile.ai[0] = 1f;
            }
            if (Projectile.alpha <= 0)
            {
                Projectile.ai[0] = 0f;
            }*/
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                float circlePos = i;
                Vector2 spawnPos = Projectile.Center + (circlePos.ToRotationVector2() * 8f);
                Vector2 velocity = spawnPos - Projectile.Center;
                velocity.Normalize();
                Dust dustIndex = Dust.NewDustPerfect(spawnPos, 21, velocity * 0.8f, Scale: Main.rand.NextFloat(0.8f, 2.2f));
                dustIndex.noGravity = true;
            }
        }
    }
}