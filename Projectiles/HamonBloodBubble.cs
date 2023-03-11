using JoJoStands.Items.Hamon;
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
            Projectile.timeLeft = 800;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 7;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (Projectile.timeLeft <= 790)
                Projectile.velocity *= 0;

            if (hamonPlayer.amountOfHamon >= 1)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 169, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
        }
    }
}