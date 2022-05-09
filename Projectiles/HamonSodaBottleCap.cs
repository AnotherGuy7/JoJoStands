using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class HamonSodaBottleCap : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 3;
            Projectile.height = 6;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffID.Confused, 120);
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 12)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 3)
            {
                Projectile.frame = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}