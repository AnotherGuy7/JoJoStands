using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class Yosarrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 9;
            Projectile.height = 29;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 900;
            Projectile.friendly = false;
            Projectile.arrow = true;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.3f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}