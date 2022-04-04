using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ViralStaffProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.magic = true;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            for (int d = 0; d < 3; d++)
            {
                Main.dust[Dust.NewDust(projectile.Center + new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)), projectile.width, projectile.height, 232)].noGravity = true;
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            NPC target = null;
            if (target == null)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        if (npc.lifeMax > 5 && !npc.friendly && projectile.Distance(npc.Center) <= 10f * 16f)
                        {
                            target = npc;
                        }
                    }
                }
            }
            if (target != null)
            {
                Vector2 vel = target.position - projectile.position;
                vel.Normalize();
                vel *= 2f;
                projectile.velocity += vel;
            }

            projectile.velocity.Y += 0.1f;
            if (projectile.velocity.Y >= 5f)
            {
                projectile.velocity.Y = 5f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X > 0f)
            {
                projectile.velocity.X -= 0.5f;
            }
            else
            {
                projectile.velocity.X += 0.5f;
            }
            projectile.velocity.Y *= -1f;
            return false;
        }
    }
}