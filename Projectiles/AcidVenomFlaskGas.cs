using JoJoStands.Buffs.AccessoryBuff;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class AcidVenomFlaskGas : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 7;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }

        private int randomrotation = 0;
        private int nodamage = 30;
        private int expertboost = 1;
        private int healzombie = 0;

        public override void AI()
        {
            if (Main.expertMode)
                expertboost = 2;

            if (randomrotation == 0)
                randomrotation = Main.rand.Next(-5, 5);

            if (nodamage > 0)
            {
                nodamage--;
                Projectile.damage = 0;
            }
            else
                Projectile.damage = 7 * expertboost;

            Projectile.rotation += MathHelper.ToRadians(randomrotation * 1f);
            Projectile.alpha += 1;
            if (Projectile.alpha >= 255)
                Projectile.Kill();
        }

        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.HasBuff(ModContent.BuffType<Vampire>()))
            {
                if (target.life < target.lifeMax)
                {
                    healzombie += 1;
                    if (healzombie == 20)
                    {
                        target.life += 3 * expertboost;
                        healzombie = 0;
                    }
                }
            }
            target.AddBuff(BuffID.Venom, 180);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 180);
        }
    }
}