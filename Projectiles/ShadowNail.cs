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
            if (projectile.position != Main.MouseWorld)
            {
                if (projectile.position.X < Main.MouseWorld.X)        //if it's more to the right, go left
                {
                    projectile.velocity.X = 5f;
                }
                else if (projectile.position.X > Main.MouseWorld.X)       //if it's more to the left, stay the same...
                {
                    projectile.velocity.X = -5f;
                }
                if (projectile.position.Y > Main.MouseWorld.Y)       //if it's lower, go up to it
                {
                    projectile.velocity.Y = -5f;
                }
                else if (projectile.position.Y < Main.MouseWorld.Y)      //if it's higher, go down to it
                {
                    projectile.velocity.Y = 5f;
                }
                if (projectile.position.Y == Main.MouseWorld.Y)
                {
                    projectile.velocity.Y = 0f;
                }
                if (projectile.position.X == Main.MouseWorld.X)
                {
                    projectile.velocity.X = 0f;
                }
            }
            if (projectile.timeLeft <= 275 && Main.mouseRight)
            {
                projectile.Kill();
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