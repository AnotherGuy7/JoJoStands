using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class ViralArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        private bool[] targetedArray = new bool[Main.maxNPCs];

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
                    bool alreadyTargeted = targetedArray[n];
                    if (npc.active)
                    {
                        if (npc.lifeMax > 5 && !npc.friendly && Projectile.Distance(npc.Center) <= 25f * 16f && !alreadyTargeted)
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
                vel *= 12f;
                Projectile.velocity = vel;
            }

            Projectile.velocity.Y += 0.1f;
            if (Projectile.velocity.Y >= 5f)
            {
                Projectile.velocity.Y = 5f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            targetedArray[target.whoAmI] = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= -1;
            Projectile.timeLeft -= 60;
            return false;
        }
    }
}