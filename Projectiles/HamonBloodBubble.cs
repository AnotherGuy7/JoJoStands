using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HamonBloodBubble : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 15 * 60;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 7;
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

            Vector2 dustSpawnPosition = Projectile.Center - (new Vector2(Projectile.width, Projectile.height) * 0.5f * Projectile.scale);
            if (hamonPlayer.amountOfHamon >= 1)
            {
                int dustIndex = Dust.NewDust(dustSpawnPosition, (int)(Projectile.width * Projectile.scale), (int)(Projectile.height * Projectile.scale), 169, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
                Main.dust[dustIndex].noGravity = true;
            }
            else
            {
                if (Projectile.timeLeft > 120)
                    Projectile.timeLeft = 120;
            }

            Dust.NewDust(dustSpawnPosition, (int)(Projectile.width * Projectile.scale), (int)(Projectile.height * Projectile.scale), DustID.Blood, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
        }
    }
}