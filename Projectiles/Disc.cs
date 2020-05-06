using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class Disc : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 36;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.scale = 0.5f;
        }

        public override void AI()
        {
            projectile.rotation = projectile.direction * 180f;
        }
    }
}