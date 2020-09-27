using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ViralWoodSharpnel : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 12;
            projectile.aiStyle = 0;
            projectile.ranged = true;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 2;
        }

        public override void AI()
        {
            for (int d = 0; d < 8; d++)
            {
                Main.dust[Dust.NewDust(projectile.Center + new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)) + projectile.velocity, projectile.width, projectile.height, 232)].noGravity = true;
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            projectile.velocity.Y += 0.1f;
            if (projectile.velocity.Y >= 5f)
            {
                projectile.velocity.Y = 5f;
            }
        }
    }
}