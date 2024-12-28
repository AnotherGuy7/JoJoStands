using JoJoStands.Buffs.Debuffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Projectiles
{
    public class ViralCrystalProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Infected>(), 360);
            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.25f);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.Next(1, 101) >= 45)
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.TreasureSparkle);
        }
    }
}