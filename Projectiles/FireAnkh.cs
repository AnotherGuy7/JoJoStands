using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class FireAnkh : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 20;
            projectile.aiStyle = 0;
            projectile.timeLeft = 360;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.penetrate = 2;
        }

        private bool playedSound = false;

        public override void AI()
        {
            if (!playedSound)
            {
                Main.PlaySound(SoundID.Item20);
                playedSound = true;
            }
            if (projectile.wet || projectile.honeyWet)
            {
                projectile.scale -= 0.05f;
            }
            if (projectile.scale <= 0f)
            {
                projectile.Kill();
            }
            int num109 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default(Color), 3.5f);
            Main.dust[num109].noGravity = true;
            Main.dust[num109].velocity *= 1.4f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(0, 101) < projectile.ai[0])
            {
                target.AddBuff(BuffID.OnFire, (int)projectile.ai[1]);
            }
        }
    }
}