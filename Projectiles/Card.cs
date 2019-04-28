using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
 
namespace JoJoStands.Projectiles
{
    public class Card : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 18;
            projectile.aiStyle = 0;         //Honestly, this is just to give D'Arby an item to sell, and to play around with frames a little
            projectile.ranged = true;
            projectile.timeLeft = 270;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 2;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            projectile.frame = Main.rand.Next(2); // either 0 or 1
        }
    }
}