using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.Minions
{  
    public class GEButterfly : ModProjectile
    {
        public override string Texture { get { return "Terraria/NPC_" + NPCID.Butterfly; } }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 24;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 45;
            projectile.height = 28;
            projectile.friendly = true;
            projectile.netImportant = true;
            projectile.timeLeft = 1200;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public NPC npcTarget = null;
        public bool Up = false;

        public override void AI()
        {
            if (projectile.direction == -1)     //sprite turns depending on direction
            {
                projectile.spriteDirection = 1;
            }
            if (projectile.direction == 1)
            {
                projectile.spriteDirection = -1;
            }
            Vector2 move = Vector2.Zero;
            float distance = 400f;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy && Main.npc[k].type != NPCID.CultistTablet)
                {
                    Vector2 newMove = Main.npc[k].Center - projectile.Center;
                    npcTarget = Main.npc[k];
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        projectile.ai[0] = 1f;
                    }
                    else
                    {
                        projectile.ai[0] = 0f;
                    }
                }
            }
            if (projectile.ai[0] == 0f)
            {
                if (projectile.velocity.Y <= -0.3f)
                {
                    Up = false;
                }
                if (projectile.velocity.Y >= 0.3f)
                {
                    Up = true;
                }
                if (!Up)
                {
                    projectile.velocity.Y += 0.05f;
                }
                if (Up)
                {
                    projectile.velocity.Y -= 0.05f;
                }
            }
            if (projectile.ai[0] == 1f)
            {
                if (projectile.velocity.X > 3f)
                {
                    projectile.velocity.X = 3f;
                }
                if (projectile.velocity.X < -3f)
                {
                    projectile.velocity.X = -3f;
                }
                if (projectile.position.Y < npcTarget.position.Y)     //if it's higher   
                {
                    projectile.velocity.Y = 1.5f;
                }
                if (projectile.position.Y > npcTarget.position.Y)     //if it's higher   
                {
                    projectile.velocity.Y = -1.5f;
                }
                if (npcTarget.position.X - 10f > projectile.position.X)
                {
                    projectile.velocity.X = 1.5f;
                    projectile.direction = 1;
                }
                if (npcTarget.position.X + 10f < projectile.position.X)
                {
                    projectile.velocity.X = -1.5f;
                    projectile.direction = -1;
                }
            }
            projectile.frameCounter++;
            if (projectile.frameCounter >= 8)
            {
                projectile.frame += 1;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 12)
            {
                projectile.frame = 9;
            }
            if (projectile.frame <= 8)
            {
                projectile.frame = 9;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}