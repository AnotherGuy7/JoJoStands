using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class SpaceRipperStingyEyes : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.maxPenetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 12)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
            }
            if (Projectile.frame >= 3)
            {
                Projectile.frame = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(0f);
        }
    }
}