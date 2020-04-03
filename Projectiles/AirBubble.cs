using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class AirBubble : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.Kill();
        }

        public override void AI()
        {
            if (projectile.ai[0] == 0f)
            {
                projectile.alpha += 6;
            }
            if (projectile.ai[0] == 1f)
            {
                projectile.alpha -= 6;
            }
            if (projectile.alpha >= 255)
            {
                projectile.ai[0] = 1f;
            }
            if (projectile.alpha <= 0)
            {
                projectile.ai[0] = 0f;
            }
        }
    }
}