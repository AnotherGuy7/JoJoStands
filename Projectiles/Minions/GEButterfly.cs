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

        public int frame = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 24;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 45;
            projectile.height = 28;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1;
            projectile.penetrate = 1;
            projectile.timeLeft = 0;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.melee = true;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
        }

        public override void AI()
        {
            NPC npcTarget = null;
            if (projectile.direction == -1)     //sprite turns depending on direction
            {
                projectile.spriteDirection = -1;
            }
            if (projectile.direction == 1)
            {
                projectile.spriteDirection = 1;
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
        }
 
        public void SelectFrame()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 8)
            {
                frame += 1;
                projectile.frameCounter = 0;
            }
            if (frame >= 12)
            {
                frame = 9;
            }
            if (frame <= 8)
            {
                frame = 9;
            }
        }
    }
}