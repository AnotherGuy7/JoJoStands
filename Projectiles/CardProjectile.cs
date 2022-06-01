using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class CardProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 10;
            Projectile.aiStyle = 0;         //Honestly, this is just to give D'Arby an Item to sell, and to play around with frames a little
            Projectile.timeLeft = 5 * 60;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 2;
        }

        private int cardType = 0;
        private bool setCardType = false;

        public override void AI()
        {
            if (!setCardType)
            {
                setCardType = true;
                cardType = Main.rand.Next(0, 1 + 1);
                Projectile.frame = 4 * cardType;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frame++;
                if (Projectile.frame >= 4 * (cardType + 1))
                    Projectile.frame = 4 * cardType;
            }
        }
    }
}