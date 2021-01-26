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
            projectile.damage = 7;
            projectile.width = 32;
            projectile.height = 32;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.friendly = false;
            projectile.hostile = true;
        }

        private int randomrotation = 0;
        private int nodamage = 30;
        private int expertboost = 1;
        private int healzombie = 0;

        public override void AI()
        {
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (!npc.hide && projectile.Colliding(projectile.Hitbox, npc.Hitbox) && npc.HasBuff(mod.BuffType("Vampire")))
                    {
                        if (npc.life < npc.lifeMax)
                        {
                            healzombie += 1;
                            if (healzombie == 20)
                            {
                                npc.life += 3 * expertboost;
                                healzombie = 0;
                            }
                        }
                    }
                }
            }
            if (Main.expertMode)
            {
                expertboost = 2;
            }
            if (nodamage > 0)
            {
                projectile.damage = 0;
                nodamage -= 1;
            }
            if (nodamage == 0)
            {
                projectile.damage = 7* expertboost;
            }
            if (randomrotation == 0)
            {
                randomrotation = Main.rand.Next(-5, 5);
            }
            projectile.rotation += MathHelper.ToRadians(randomrotation * 1f);
            projectile.alpha += 1;
            if (projectile.alpha == 255)
            {
                projectile.Kill();
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, 180);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 180);
        }
    }
}