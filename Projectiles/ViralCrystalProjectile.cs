using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class ViralCrystalProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 14;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("Infected"), 360);
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            if (Main.rand.Next(1, 101) >= 45)
                Dust.NewDust(projectile.Center, projectile.width, projectile.height, 204);
        }
    }
}