using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class CrystalShard : ModProjectile
    {
        public int shardType = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 28;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1800;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.maxPenetrate = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            Projectile.velocity.Y += 0.3f;
            if (shardType == 0)
            {
                shardType = Main.rand.Next(0, 3);
            }
            Projectile.frame = shardType;
            Projectile.damage += (3 - shardType) * 10;
        }
    }
}