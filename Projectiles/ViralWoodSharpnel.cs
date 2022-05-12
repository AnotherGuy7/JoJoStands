using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ViralWoodSharpnel : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 12;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 2;
        }

        public override void AI()
        {
            for (int d = 0; d < 8; d++)
            {
                Main.dust[Dust.NewDust(Projectile.Center + new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)) + Projectile.velocity, Projectile.width, Projectile.height, 232)].noGravity = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            Projectile.velocity.Y += 0.1f;
            if (Projectile.velocity.Y >= 5f)
                Projectile.velocity.Y = 5f;
        }
    }
}