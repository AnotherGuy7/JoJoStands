using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class AcidVenomFlask : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 21;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }

        private int expertboost = 1;

        public override void AI()
        {
            if (Main.expertMode)
                expertboost = 2;

            Projectile.rotation += MathHelper.ToRadians(20f);
            Projectile.velocity.Y += 0.33f;

            Projectile.damage = 21 * expertboost;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 300);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 300);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107, Projectile.Center);
            Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.oldVelocity * 0.2f, 704);

            int gasclouds = Main.rand.Next(21, 30);
            for (int index = 0; index < gasclouds; ++index)
            {
                Vector2 gasVelocity = new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-100, 100));
                gasVelocity.Normalize();
                gasVelocity *= (float)Main.rand.Next(10, 70) * 0.007f;
                int acidVenomGas = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, gasVelocity, ModContent.ProjectileType<AcidVenomFlaskGas>(), 7 * expertboost, 1f, Projectile.owner);
                Main.projectile[acidVenomGas].netUpdate = true;
            }
        }
    }
}