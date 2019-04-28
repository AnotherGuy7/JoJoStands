using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class SteelBallP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            
            projectile.CloneDefaults(ProjectileID.LightDisc);
            projectile.width = 32;
            projectile.height = 32;
            projectile.melee = true;
            projectile.aiStyle = 3;
            projectile.scale = 0.5f;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.maxPenetrate = 2;
            projectile.rotation = 0f;
            Main.projFrames[projectile.type] = 7;

        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(mod.BuffType("Spin"), 120);
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.oldVelocity = new Vector2(0);
            return base.OnTileCollide(oldVelocity);
        }

        public override void AI()
        {
            projectile.frame++;
            if (projectile.frame >= Main.projFrames[projectile.type]) projectile.frame = 0;

            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 128, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
        }
    }
}