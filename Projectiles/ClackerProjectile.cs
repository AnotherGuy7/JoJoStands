using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class ClackerProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.WoodenBoomerang);
            projectile.width = 18;
            projectile.height = 12;
            projectile.melee = true;
            projectile.aiStyle = 3;
            projectile.ranged = true;
            projectile.timeLeft = 180;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.maxPenetrate = 2;
            Main.projFrames[projectile.type] = 3;
        }

        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            if (target.HasBuff(mod.BuffType("Vampire")))
            {
                target.AddBuff(mod.BuffType("Sunburn"), 120);
            }
            base.ModifyHitPvp(target, ref damage, ref crit);
        }

        public override void AI()
        {
            projectile.rotation *= 0f;
            projectile.frame++;
            if (projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
        }
    }
}