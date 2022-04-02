using System;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class StoneFreeBindString : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.timeLeft = 360;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()       //all this so that the other chain doesn't draw... yare yare. It was mostly just picking out types
        {
            float direction = projectile.velocity.X / Math.Abs(projectile.velocity.X);
            if (direction > 0)
                projectile.direction = -1;
            if (direction < 0)
                projectile.direction = 1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss && !target.hide && !target.immortal)
                target.GetGlobalNPC<NPCs.JoJoGlobalNPC>().boundByStrings = true;

            projectile.Kill();
        }
    }
}