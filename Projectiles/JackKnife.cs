using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class JackKnife : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.timeLeft = 1800;
            projectile.friendly = false;
            projectile.tileCollide = true;
            projectile.hostile = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
        }
    }
}