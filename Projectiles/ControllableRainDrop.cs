using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ControllableRainDrop : ModProjectile
    {
        public override string Texture => "JoJoStands/Projectiles/ControllableRainDrop";

        private const float SPEED = 14f;
        private const float TURN_SPEED = 0.22f;
        private const float GRAVITY = 0.18f;
        private const float MAX_FALL = 18f;

        private bool controlled = true;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 360;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 40;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.owner == Main.myPlayer)
            {
                if (Main.mouseRight && controlled)
                {
                    Vector2 toMouse = Main.MouseWorld - Projectile.Center;
                    if (toMouse.LengthSquared() > 4f)
                    {
                        toMouse.Normalize();
                        toMouse *= SPEED;
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, toMouse, TURN_SPEED);
                    }
                    Projectile.netUpdate = true;
                }
                else
                {
                    controlled = false;
                    Projectile.velocity.Y += GRAVITY;
                    if (Projectile.velocity.Y > MAX_FALL) Projectile.velocity.Y = MAX_FALL;
                }
            }
            else
            {
                Projectile.velocity.Y += GRAVITY;
                if (Projectile.velocity.Y > MAX_FALL) Projectile.velocity.Y = MAX_FALL;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(2))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f,
                    120, default, Main.rand.NextFloat(0.65f, 1.0f));
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.boss) target.velocity *= 0.4f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 8; i++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2.5f, -0.5f),
                    0, default, Main.rand.NextFloat(0.9f, 1.2f));
            return true;
        }
    }
}
