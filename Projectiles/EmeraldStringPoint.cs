using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class EmeraldStringPoint : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public const float MaxUnconnectedDistance = 32f * 16f;

        public override void AI()
        {
            if (projectile.ai[0] == 1f)
                projectile.timeLeft = 2;

            if (projectile.ai[0] == 0f)
            {
                if (projectile.Distance(Main.player[projectile.owner].Center) >= MaxUnconnectedDistance)
                {
                    projectile.Kill();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}