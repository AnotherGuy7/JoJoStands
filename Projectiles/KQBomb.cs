using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class KQBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.GrenadeIV);
            projectile.width = 20;
            projectile.height = 18;
            projectile.ranged = true;
            projectile.timeLeft = 900;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.scale = 0.6f;
        }

        public override void AI()
        {
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Fire, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
            if (projectile.active && Main.mouseRight)
            {
                projectile.timeLeft = 1;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.FlameBurst, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item62);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
    }
}