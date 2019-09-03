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
        public int targetTimer = 600;

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

        protected float shootCool = 80f;       //how fast the minion can shoot
        protected float shootSpeed = 9f;     //how fast the projectile the minion shoots goes
        static bool normalFrames = false;
        static bool attackFrames = false;

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
                    if (Main.npc[k].type == mod.NPCType("Priest"))
                    {
                        pucci = Main.npc[k];
                    }
                }
            }
            targetTimer--;
            if (projectile.active)
            {
                whitesnakeActive = true;
            }
            if (targetTimer <= 0 || !NPCs.Priest.userIsAlive)
            {
                projectile.alpha++;
            }
            if (projectile.alpha >= 255)
            {
                whitesnakeActive = false;
                projectile.active = false;
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
            projectile.tileCollide = true;
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
                        attackFrames = true;
                        normalFrames = false;
                        targetTimer = 600;
                    }
                    else
                    {
                        attackFrames = false;
                    }
                }
            }
            SelectFrame();
            if (projectile.ai[1] > 0f)
            {
                projectile.ai[1] += 1f;
                if (Main.rand.Next(3) == 0)
                {
                    projectile.ai[1] += 1f;
                }
            }
            if (projectile.ai[1] > shootCool)
            {
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }
            if (projectile.ai[0] == 0f)
            {
                if (target)
                {
                    if ((targetPos - projectile.Center).X > 0f)
                    {
                        projectile.spriteDirection = (projectile.direction = 1);
                    }
                    else if ((targetPos - projectile.Center).X < 0f)
                    {
                        projectile.spriteDirection = (projectile.direction = -1);
                    }
                    if (projectile.ai[1] == 0f)
                    {
                        projectile.ai[1] = 1f;
                        Vector2 shootVel = targetPos - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Disc"), 42, 1f, Main.myPlayer, 0, 0);
                        Main.projectile[proj].npcProj = true;
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                }
            }
            if (projectile.frame >= 4 && projectile.frame <= 7)
            {
                projectile.ai[0] = 0f;
            }
            else
            {
                projectile.ai[0] = 1f;
            }
        }

        public virtual void SelectFrame()       //you can't forget that there is a frame 0!!!
        {
            if (attackFrames)       //2-3 are the attack frames
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
            if (normalFrames)       //first 2 frames are the idle frames
            {
                attackFrames = false;
                projectile.frame = 1;
            }
        }
    }
}