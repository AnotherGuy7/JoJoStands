using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class GoldExperienceRock : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 15;
            projectile.height = 6;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale = 1.5f;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(projectile.position, Vector2.Zero, mod.ProjectileType("GEScorpion"), 1, 0f, Main.myPlayer);
            Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire);
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }
    }
}