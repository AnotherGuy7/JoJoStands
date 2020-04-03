using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class Nail : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/GeneralBullet"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 12;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }
    }
}