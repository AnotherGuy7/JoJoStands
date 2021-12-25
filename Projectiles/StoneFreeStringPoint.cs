using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StoneFreeStringPoint : ModProjectile
    {
        public override string Texture => mod.Name + "/Projectiles/PlayerStands/StandPlaceholder";

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}