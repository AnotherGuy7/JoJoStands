using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class PreciseRainDrop : ModProjectile
    {
        public override string Texture => "JoJoStands/Projectiles/RainDrop";

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 50;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.28f;
            if (Projectile.velocity.Y > 18f) Projectile.velocity.Y = 18f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(2))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water, Projectile.velocity.X * 0.08f, Projectile.velocity.Y * 0.08f,
                    140, default, Main.rand.NextFloat(0.65f, 0.95f));
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.boss) target.velocity *= 0.5f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 7; i++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2.5f, -0.5f),
                    0, default, Main.rand.NextFloat(0.85f, 1.15f));
            return true;
        }
    }
}
