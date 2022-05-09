using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
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

        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
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
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Projectile.Kill();
        }
    }
}