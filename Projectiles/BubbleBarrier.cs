using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class BubbleBarrier : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile ownerProj = Main.projectile[(int)Projectile.ai[1]];
            Projectile.Center = player.Center;
        }
    }
}
   