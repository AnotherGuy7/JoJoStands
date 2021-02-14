using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HamonBubble : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = 0;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (projectile.timeLeft <= 592)
            {
                projectile.velocity = Vector2.Zero;
            }
            if (hamonPlayer.amountOfHamon >= 1)
            {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 169, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            }
            else
            {
                projectile.timeLeft /= 2;
            }
        }
    }
}