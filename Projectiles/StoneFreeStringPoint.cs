using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StoneFreeStringPoint : ModProjectile
    {
        public override string Texture => Mod.Name + "/Projectiles/PlayerStands/StandPlaceholder";

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}