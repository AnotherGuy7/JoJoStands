using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.NovemberRain
{
    public class RainDrop : ModProjectile
    {
        public override string Texture => "JoJoStands/Projectiles/RainDrop";

        private float SlowMultiplier => Projectile.ai[0]; // StandT'den geçirilir

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 80;
            Projectile.extraUpdates = 1; // Hızlı hareket için
        }

        public override void AI()
        {
            // Su damlası görsel efekti
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water, 0f, 0f, 150, default, 0.7f
                );
                dust.noGravity = true;
                dust.velocity = Projectile.velocity * 0.2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Boss olmayan düşmanları yavaşlat
            if (!target.boss)
                target.velocity *= SlowMultiplier;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Zemine çarptığında su sıçraması
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(
                    Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water,
                    Main.rand.NextFloat(-1.5f, 1.5f), -1f,
                    0, default, 0.9f
                );
            }
            return true;
        }
    }
}
