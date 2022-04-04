using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class GoldExperienceBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 8;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(projectile.position, Vector2.Zero, mod.ProjectileType("GEScorpion"), 1, 0f, Main.myPlayer);
            int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 169);
            Main.dust[dustIndex].noGravity = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
        }
    }
}