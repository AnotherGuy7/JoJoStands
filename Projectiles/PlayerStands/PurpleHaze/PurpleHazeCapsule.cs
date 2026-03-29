using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.PurpleHaze
{
    public class PurpleHazeCapsule : ModProjectile
    {
        private ref float HasBurst => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.light = 0.4f;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.55f;

            if (Projectile.velocity.Y > 22f)
                Projectile.velocity.Y = 22f;

            Projectile.rotation += 0.18f * (Projectile.velocity.X > 0 ? 1f : -1f);

            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.PurpleTorch, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f,
                    120, new Color(200, 80, 255), Main.rand.NextFloat(0.8f, 1.4f));
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HazeVirusCloud>(), 0, 0f, Projectile.owner);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            TriggerBurst(Projectile.Center);
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            TriggerBurst(Projectile.Center);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            TriggerBurst(Projectile.Center);
        }

        private void TriggerBurst(Vector2 position)
        {
            if (HasBurst >= 1f || Projectile.owner != Main.myPlayer)
                return;

            HasBurst = 1f;
            Projectile.netUpdate = true;

            if (Main.netMode != NetmodeID.Server)
            {
                int popCount = 24;
                for (int i = 0; i < popCount; i++)
                {
                    float angle = MathHelper.TwoPi / popCount * i;
                    float speed = Main.rand.NextFloat(3f, 7f);
                    Vector2 vel = new Vector2(speed, 0f).RotatedBy(angle);
                    int d = Dust.NewDust(position, 0, 0, DustID.PurpleTorch,
                        vel.X, vel.Y, 0, new Color(220, 100, 255),
                        Main.rand.NextFloat(1.2f, 2f));
                    Main.dust[d].noGravity = true;
                }

                for (int i = 0; i < 16; i++)
                {
                    float angle = MathHelper.TwoPi / 16 * i;
                    float speed = Main.rand.NextFloat(1.5f, 4f);
                    Vector2 vel = new Vector2(speed, 0f).RotatedBy(angle);
                    int d = Dust.NewDust(position, 4, 4, DustID.Smoke,
                        vel.X, vel.Y, 160, new Color(100, 20, 160),
                        Main.rand.NextFloat(1f, 1.8f));
                    Main.dust[d].noGravity = true;
                }
            }
        }
    }
}