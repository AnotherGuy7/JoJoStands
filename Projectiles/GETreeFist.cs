using Terraria.ModLoader;
using Terraria;
 
namespace JoJoStands.Projectiles
{
    public class GETreeFist : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.aiStyle = 0;
            projectile.timeLeft = 20;
            projectile.alpha = 255;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }
    }
}