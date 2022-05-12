using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ViralStaffProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            for (int d = 0; d < 3; d++)
            {
                Main.dust[Dust.NewDust(Projectile.Center + new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)), Projectile.width, Projectile.height, 232)].noGravity = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            NPC target = null;
            if (target == null)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        if (npc.lifeMax > 5 && !npc.friendly && Projectile.Distance(npc.Center) <= 10f * 16f)
                        {
                            target = npc;
                        }
                    }
                }
            }
            if (target != null)
            {
                Vector2 vel = target.position - Projectile.position;
                vel.Normalize();
                vel *= 2f;
                Projectile.velocity += vel;
            }

            Projectile.velocity.Y += 0.1f;
            if (Projectile.velocity.Y >= 5f)
            {
                Projectile.velocity.Y = 5f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X > 0f)
            {
                Projectile.velocity.X -= 0.5f;
            }
            else
            {
                Projectile.velocity.X += 0.5f;
            }
            Projectile.velocity.Y *= -1f;
            return false;
        }
    }
}