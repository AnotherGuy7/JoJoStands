using System;
using System.Reflection;
using JoJoStands.NPCs;
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
        }

        private const float DetectionDistance = 16f * 16f;
        private NPC npcTarget = null;
        private bool Up = false;

        public override void AI()
        {
            projectile.spriteDirection = -projectile.direction;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.lifeMax > 5 && npc.type != NPCID.TargetDummy && npc.type != NPCID.CultistTablet)
                {
                    float distance = projectile.Distance(npc.Center);
                    if (distance < DetectionDistance)
                    {
                        npcTarget = npc;
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.GetGlobalNPC<JoJoGlobalNPC>().taggedByButterfly = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}