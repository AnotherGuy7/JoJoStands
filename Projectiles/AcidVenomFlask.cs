using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class AcidVenomFlask : ModProjectile
    {
        private int expertboost = 1;

        public override void SetDefaults()
        {
            projectile.damage = 21;
            projectile.width = 18;
            projectile.height = 18;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
        }
        public override void AI()
        {
            if (Main.expertMode)
            {
                expertboost = 2;
            }

            projectile.rotation += MathHelper.ToRadians(20f);
            projectile.velocity.Y += 0.33f;

            projectile.damage = 21 * expertboost;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 107);
            Gore.NewGore(projectile.position, projectile.oldVelocity * 0.2f, 704);
            int gasclouds = Main.rand.Next(21, 30);
            for (int index = 0; index < gasclouds; ++index)
            {
                Vector2 gas = new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-100, 100));
                gas.Normalize();
                gas *= (float)Main.rand.Next(10, 70) * 0.007f;
                int acidvenomgas = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, gas.X, gas.Y, mod.ProjectileType("AcidVenomFlaskGas"), 7*expertboost, 1f);
                Main.projectile[acidvenomgas].netUpdate = true;
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.Kill();
            target.AddBuff(BuffID.Venom, 300);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.Kill();
            target.AddBuff(BuffID.Venom, 300);
        }
    }
}