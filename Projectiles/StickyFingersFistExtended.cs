using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class StickyFingersFistExtended : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 25;
            projectile.height = 30;
            projectile.aiStyle = 15;
            projectile.ranged = true;
            projectile.timeLeft = 30;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 2;
            projectile.maxPenetrate = 4;
        }

        public override void AI()
        {
            projectile.velocity.Y += 0.3f;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }
    }
}