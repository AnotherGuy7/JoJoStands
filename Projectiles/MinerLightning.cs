using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class MinerLightning : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 8;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 61);
            Main.dust[dustIndex].noGravity = true;
        }
    }
}