using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.NPCStands
{  
    public class StarPlatinumPart4 : ModProjectile
    {
        public static bool SPActive = false;
        public int targetTimer = 600;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
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
            projectile.height = 74;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.npcProj = true;
        }

        protected float shootCool = 6f;       //how fast the minion can shoot
        protected float shootSpeed = 9f;     //how fast the projectile the minion shoots goes
        static bool normalFrames = false;
        static bool attackFrames = false;

        public override void Kill(int timeLeft)
        {
            SPActive = false;
        }

        public override void AI()
        {
            NPC jotaro = null;
            if (NPCs.MarineBiologist.userIsAlive)
            {
                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];
                    if (Main.npc[k].type == mod.NPCType("MarineBiologist"))
                    {
                        jotaro = Main.npc[k];
                    }
                }
            }
            targetTimer--;
            if (projectile.active)
            {
                SPActive = true;
            }
            if (targetTimer <= 0 || !NPCs.MarineBiologist.userIsAlive)
            {
                projectile.Kill();
            }
            if (jotaro != null)
            {
                Vector2 vector131 = jotaro.Center;
                vector131.X -= (float)((5 + jotaro.width / 2) * jotaro.direction);
                vector131.Y -= 15f;
                projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                projectile.velocity *= 0.8f;
                projectile.direction = (projectile.spriteDirection = jotaro.direction);
            }

            Vector2 targetPos = projectile.position;
            float targetDist = 98f;
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
                        targetTimer = 600;
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
                    attackFrames = true;
                    normalFrames = false;
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
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Fists"), NPCs.MarineBiologist.attackPower, 1f, Main.myPlayer, 0, 0);
                        Main.projectile[proj].npcProj = true;
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                }
                else
                {
                    attackFrames = false;
                }
            }
        }

        public virtual void SelectFrame()
        {
            if (attackFrames)
            {
                normalFrames = false;
                projectile.frameCounter++;
                if (projectile.frameCounter >= 6)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 4)
                {
                    projectile.frame = 2;
                }
            }
            if (normalFrames)
            {
                attackFrames = false;
                projectile.frameCounter++;
                if (projectile.frameCounter >= 14)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
        }
    }
}