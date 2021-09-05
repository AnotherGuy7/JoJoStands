using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ReqNail : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/ControllableNail"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 6;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("Spin"), 60);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("Spin"), 60);
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();

            int pinkDustIndex = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 205, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            Main.dust[pinkDustIndex].noGravity = true;

            int blueDustIndex = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 202, projectile.velocity.X * -0.3f, projectile.velocity.Y * -0.3f);
            Main.dust[blueDustIndex].noGravity = true;
        }
    }
}