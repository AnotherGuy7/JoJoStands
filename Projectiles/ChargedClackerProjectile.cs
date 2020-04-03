using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class ChargedClackerProjectile : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/ClackerProjectile"; }
        }

        public override void SetStaticDefaults()        //find the autoload thing for the texture and make it the same as the clacker one
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            
            projectile.CloneDefaults(ProjectileID.WoodenBoomerang);
            projectile.width = 18;
            projectile.height = 12;
            projectile.aiStyle = 3;
            projectile.ranged = true;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.maxPenetrate = 4;
            Main.projFrames[projectile.type] = 3;
        }

        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            if (target.HasBuff(mod.BuffType("Vampire")))
            {
                target.AddBuff(mod.BuffType("Sunburn"), 240);
            }
            base.ModifyHitPvp(target, ref damage, ref crit);
        }

        public override void AI()
        {
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 169, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            projectile.rotation *= 0f;
            projectile.frame++;
            if (projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;
        }
    }
}