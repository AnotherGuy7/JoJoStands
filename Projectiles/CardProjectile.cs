using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class CardProjectile : ModProjectile
    {
        private bool setFrame = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 18;
            Projectile.aiStyle = 0;         //Honestly, this is just to give D'Arby an Item to sell, and to play around with frames a little
            Projectile.ranged = true;
            Projectile.timeLeft = 270;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 2;
            Projectile.scale = 0.5f;
        }


        public override void AI()
        {
            if (!setFrame)
            {
                Projectile.frame = Main.rand.Next(2);
                setFrame = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }
    }
}