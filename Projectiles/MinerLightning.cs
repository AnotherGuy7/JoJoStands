using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class MinerLightning : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 8;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 61);
            Main.dust[dustIndex].noGravity = true;
        }
    }
}