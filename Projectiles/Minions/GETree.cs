using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.Minions
{  
    public class GETree : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 13;
        }

        public bool timeLeftDeclared = false;
        public bool shrinkAndDie = false;

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 41;
            projectile.friendly = true;
            projectile.penetrate = 9999;
            projectile.timeLeft = 5;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale = 3f;
            drawOriginOffsetX = -10;
            drawOriginOffsetY = 41;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public override void AI()
        {
            projectile.velocity.X = 0f;
            projectile.velocity.Y = 3f;
            projectile.direction = -1;
            if (projectile.ai[0] == 0f && !timeLeftDeclared)
            {
                projectile.timeLeft = 900;
                timeLeftDeclared = true;
            }
            if (projectile.ai[0] == 1f && !timeLeftDeclared)
            {
                projectile.timeLeft = 1200;
                timeLeftDeclared = true;
            }
            if (projectile.ai[0] == 2f && !timeLeftDeclared)
            {
                projectile.timeLeft = 1500;
                timeLeftDeclared = true;
            }
            if (projectile.ai[0] == 3f && !timeLeftDeclared)
            {
                projectile.timeLeft = 1800;
                timeLeftDeclared = true;
            }
            if (projectile.timeLeft <= 181)
            {
                shrinkAndDie = true;
            }
            if (!shrinkAndDie)
            {
                if (projectile.frame <= 11)
                {
                    projectile.frameCounter++;
                }
                if (projectile.frameCounter >= 13.85)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
            }
            if (shrinkAndDie)
            {
                projectile.frameCounter++;
                if (projectile.frameCounter >= 13.85)
                {
                    projectile.frame -= 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 0)
                {
                    projectile.Kill();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = target.damage;
            knockback = -target.velocity.X;      //they're just gonna have to go back as fast as they were going
        }
    }
}