using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class HamonBubble : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (Projectile.timeLeft <= 592)
            {
                Projectile.velocity = Vector2.Zero;
            }
            if (hamonPlayer.amountOfHamon >= 1)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
                Main.dust[dustIndex].noGravity = true;;
            }
            else
            {
                Projectile.timeLeft /= 2;
            }
        }
    }
}