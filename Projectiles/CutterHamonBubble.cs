using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class CutterHamonBubble : ModProjectile
    {
        private int hamonLossCounter = 0;
        private bool beingControlled = false;

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 2;
            Projectile.maxPenetrate = 3;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Items.Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Items.Hamon.HamonPlayer>();
            if (!Main.mouseRight)
            {
                Projectile.Kill();
                return;
            }

            hamonLossCounter++;
            if (beingControlled)
            {
                hamonLossCounter++;
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.velocity = Main.MouseWorld - Projectile.Center;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 10f;
                }
                Projectile.netUpdate = true;
            }
            if (hamonLossCounter >= 120)
            {
                hamonLossCounter = 0;
                hamonPlayer.amountOfHamon -= 1;
            }
            if (hamonPlayer.amountOfHamon <= 1)
            {
                beingControlled = false;
                Projectile.timeLeft--;
            }
            else
            {
                int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 169, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
                Main.dust[dustIndex].noGravity = true;
            }
        }
    }
}