using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class FireAnkh : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;      //had to give it a bad hitbox cause it wasn't hitting anything
            projectile.height = 20;
            projectile.aiStyle = 0;
            projectile.ranged = true;
            projectile.timeLeft = 360;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.penetrate = 2;
        }

        public override void AI()
        {
            if (projectile.wet || projectile.honeyWet)
            {
                projectile.scale -= 0.05f;
            }
            if (projectile.scale <= 0f)
            {
                projectile.timeLeft = 1;
            }
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 35, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
        }
    }
}