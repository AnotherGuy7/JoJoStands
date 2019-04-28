using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ReqNail : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.ranged = true;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("Spin"), 1, true);
            target.velocity.Y += 80f;
            target.noTileCollide = true;
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 205, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
        }
    }
}