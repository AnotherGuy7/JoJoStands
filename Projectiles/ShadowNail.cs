using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class ShadowNail : ModProjectile
    {
        public override void SetDefaults()      //I couldn't get it to stick to tiles so I will leave this for another time
        {
            projectile.width = 25;
            projectile.height = 25;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.rotation += (float)projectile.direction * 0.8f;
            if (projectile.owner == Main.myPlayer)
            {
                projectile.netUpdate = true;
                projectile.velocity = Main.MouseWorld - projectile.position;
                projectile.velocity.Normalize();
                projectile.velocity *= 5f;
                if (projectile.timeLeft <= 275 && Main.mouseRight)
                {
                    projectile.Kill();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool PreKill(int timeLeft)
        {
            Main.player[projectile.owner].position = projectile.position;
            return false;
        }
    }
}