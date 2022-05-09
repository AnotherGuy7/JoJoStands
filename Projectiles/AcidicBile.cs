using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles
{
    public class AcidicBile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            Projectile.velocity.Y += 0.3f;

            if (Main.rand.Next(0, 2 + 1) == 0)
            {
                int newDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 39);
                Main.dust[newDust].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.GetGlobalNPC<NPCs.JoJoGlobalNPC>().zombieHightlightTimer += 45 * 60;
        }
    }
}