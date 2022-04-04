using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class SpaceRipperStingyEyes : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.penetrate = -1;
            projectile.maxPenetrate = -1;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 12)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
            }
            if (projectile.frame >= 3)
            {
                projectile.frame = 0;
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(0f);
        }
    }
}