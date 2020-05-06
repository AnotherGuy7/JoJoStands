using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.NPCStands
{  
    public class Whitesnake : ModProjectile
    {
        public static bool whitesnakeActive = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 40;
            projectile.height = 64;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.npcProj = true;
        }

        private int shootTime = 30;
        private float shootSpeed = 9f;     //how fast the projectile the minion shoots goes
        private bool normalFrames = false;
        private bool attackFrames = false;
        private float shootCount = 0f;

        public override void Kill(int timeLeft)
        {
            whitesnakeActive = false;
        }

        public override void AI()
        {
            NPC pucci = null;
            if (NPCs.Priest.userIsAlive)
            {
                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];
                    if (npc.type == mod.NPCType("Priest"))
                    {
                        pucci = npc;
                    }
                }
            }
            if (shootCount > 0)
            {
                shootCount--;
            }
            if (projectile.active)
            {
                whitesnakeActive = true;
            }
            if (!NPCs.Priest.userIsAlive)
            {
                projectile.Kill();
            }
            if (projectile.timeLeft < 256)
            {
                projectile.alpha = -projectile.timeLeft + 255;
            }
            if (pucci != null)
            {
                Vector2 vector131 = pucci.Center;
                vector131.X -= (float)((5 + pucci.width / 2) * pucci.direction);
                vector131.Y -= 15f;
                projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                projectile.velocity *= 0.8f;
                projectile.direction = (projectile.spriteDirection = pucci.direction);
            }

            Vector2 targetPos = projectile.position;
            float targetDist = 150f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                NPC npc = Main.npc[k];
                normalFrames = true;
                if (npc.CanBeChasedBy(this, false))
                {
                    float distance = Vector2.Distance(npc.Center, projectile.Center);
                    if ((distance < targetDist || !target) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                    {
                        targetDist = distance;
                        targetPos = npc.Center;
                        target = true;
                        projectile.timeLeft = 600;
                        projectile.alpha = 0;
                    }
                }
            }
            SelectFrame();
            attackFrames = target;
            if (target && shootCount <= 0 && projectile.frame == 4)
            {
                shootCount += shootTime;
                if ((targetPos - projectile.Center).X > 0f)
                {
                    projectile.spriteDirection = projectile.direction = 1;
                }
                else if ((targetPos - projectile.Center).X < 0f)
                {
                    projectile.spriteDirection = projectile.direction = -1;
                }
                Vector2 shootVel = targetPos - projectile.Center;
                if (shootVel == Vector2.Zero)
                {
                    shootVel = new Vector2(0f, 1f);
                }
                shootVel.Normalize();
                shootVel *= shootSpeed;
                int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Disc"), 40, 4f);
                Main.projectile[proj].npcProj = true;
                Main.projectile[proj].netUpdate = true;
                projectile.netUpdate = true;
            }
        }

        public virtual void SelectFrame()
        {
            if (attackFrames)
            {
                normalFrames = false;
                projectile.frameCounter++;
                if (projectile.frameCounter >= 10)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 8)
                {
                    projectile.frame = 0;
                }
            }
            if (normalFrames)
            {
                attackFrames = false;
                projectile.frame = 1;
            }
        }
    }
}