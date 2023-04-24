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
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 12 * 60;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private float bubbleRotationAddition = 0f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (Projectile.timeLeft <= (12 * 60) - 10)
                Projectile.velocity *= 0.84f;
            if (bubbleRotationAddition == 0f)
                bubbleRotationAddition = Main.rand.Next(1, 9 + 1) / 200f;

            Projectile.rotation += bubbleRotationAddition;
            Projectile.scale = MathHelper.Clamp(Projectile.timeLeft / 120f, 0f, 1f);

            if (hamonPlayer.amountOfHamon >= 1)
            {
                Vector2 spawnPosition = Projectile.Center - (new Vector2(Projectile.width, Projectile.height) * 0.5f * Projectile.scale);
                int dustIndex = Dust.NewDust(spawnPosition, (int)(Projectile.width * Projectile.scale), (int)(Projectile.height * Projectile.scale), 169, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
                Main.dust[dustIndex].noGravity = true;
            }
            else
            {
                if (Projectile.timeLeft > 120)
                    Projectile.timeLeft = 120;
            }
        }
    }
}