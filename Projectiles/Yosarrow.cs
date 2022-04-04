using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class Yosarrow : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 9;
            projectile.height = 29;
            projectile.aiStyle = 1;
            projectile.timeLeft = 900;
            projectile.friendly = false;
            projectile.arrow = true;
            projectile.hostile = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.velocity.Y += 0.3f;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}