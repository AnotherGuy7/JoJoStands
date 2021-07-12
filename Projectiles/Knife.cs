using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class Knife : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 28;
            projectile.aiStyle = 0;
            projectile.timeLeft = 1800;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.maxPenetrate = 25;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            projectile.velocity.Y += 0.3f;
            if (projectile.velocity.X <= 0)
            {
                projectile.spriteDirection = -1;
                projectile.rotation += MathHelper.ToRadians(90f);
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<MyPlayer>();
            target.immune[projectile.owner] = 0;
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 6); i++)
            {
                Dust.NewDust(projectile.Center, projectile.width / 2, projectile.height / 2, DustID.Lead, -projectile.velocity.X * 0.1f, -projectile.velocity.Y * 0.1f);
            }
            Main.PlaySound(0, (int)projectile.Center.X, (int)projectile.Center.Y, 1);
        }
    }
}