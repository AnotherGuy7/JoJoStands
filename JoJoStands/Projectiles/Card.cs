using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class Card : ModProjectile
    {
        private bool setFrame = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
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
            projectile.scale = 0.5f;
        }


        public override void AI()
        {
            if (!setFrame)
            {
                projectile.frame = Main.rand.Next(2);
                setFrame = true;
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }
    }
}