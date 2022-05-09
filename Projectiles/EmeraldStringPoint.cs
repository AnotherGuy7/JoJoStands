using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class EmeraldStringPoint : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public const float MaxUnconnectedDistance = 32f * 16f;

        public override void AI()
        {
            if (Projectile.ai[0] == 1f)
                Projectile.timeLeft = 2;

            if (Projectile.ai[0] == 0f)
            {
                if (Projectile.Distance(Main.player[Projectile.owner].Center) >= MaxUnconnectedDistance)
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}