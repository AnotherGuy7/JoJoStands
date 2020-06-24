using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class CrystalShard : ModProjectile
    {
        public int shardType = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 28;
            projectile.aiStyle = 0;
            projectile.timeLeft = 1800;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.maxPenetrate = 1;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            projectile.velocity.Y += 0.3f;
            if (shardType == 0)
            {
                shardType = Main.rand.Next(0, 3);
            }
            projectile.frame = shardType;
            projectile.damage += (3 - shardType) * 10;
        }
    }
}