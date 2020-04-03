using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class HamonTequilaBottleCap : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 3;
            projectile.height = 6;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.penetrate = 3;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffID.Confused, 120);
        }

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 12)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 3)
            {
                projectile.frame = 0;
            }
            projectile.rotation = projectile.velocity.ToRotation();
        }
    }
}