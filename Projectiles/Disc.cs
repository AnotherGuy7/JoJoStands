using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class Disc : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.hostile = false;
            Projectile.scale = 0.5f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.direction * 180f;
        }
    }
}