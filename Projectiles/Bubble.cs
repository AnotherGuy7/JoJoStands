using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class Bubble : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 18;
            projectile.aiStyle = 0;
            projectile.ranged = true;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 7;
        }

        public override void AI()
        {
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 21, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.damage = 201;
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public virtual void OnHitAnything()
        {
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 17, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.LocalPlayer;
            var p = Projectile.NewProjectile(projectile.position, projectile.velocity, ProjectileID.GrenadeIII, 150, 8f, player.whoAmI);
            Main.projectile[p].timeLeft = 2;
            Main.projectile[p].netUpdate = true;
            Main.PlaySound(SoundID.Item62);
        }
    }
}