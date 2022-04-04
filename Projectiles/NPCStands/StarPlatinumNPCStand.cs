using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.NPCs.TownNPCs;

namespace JoJoStands.Projectiles.NPCStands
{
    public class StarPlatinumNPCStand : ModProjectile
    {
        public static bool SPActive = false;

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

        private const float MaximumDetectionDistance = 6f * 16f;

        private float shootCool = 6f;       //how fast the minion can shoot
        private float shootSpeed = 9f;     //how fast the projectile the minion shoots goes
        private bool normalFrames = false;
        private bool attackFrames = false;

        public override void AI()
        {
            NPC jotaro = null;
            if (MarineBiologist.userIsAlive)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.type == mod.NPCType("MarineBiologist"))
                    {
                        jotaro = npc;
                        break;
                    }
                }
            }
            else
            {
                projectile.Kill();
            }

            SPActive = projectile.active;
            projectile.tileCollide = true;
            if (projectile.spriteDirection == 1)
            {
                drawOffsetX = -10;
            }
            if (projectile.spriteDirection == -1)
            {
                drawOffsetX = -60;
            }
            if (jotaro != null)
            {
                Vector2 vector131 = jotaro.Center;
                vector131.X -= (float)((15 + jotaro.width / 2) * jotaro.direction);
                vector131.Y -= 15f;
                projectile.Center = Vector2.Lerp(projectile.Center, vector131, 0.2f);
                projectile.velocity *= 0.8f;
                projectile.direction = (projectile.spriteDirection = jotaro.direction);
            }
            if (projectile.timeLeft < 256)
            {
                projectile.alpha = -projectile.timeLeft + 255;
            }

            NPC target = null;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                normalFrames = true;
                NPC npc = Main.npc[n];
                if (npc.CanBeChasedBy(this, false))
                {
                    float distance = Vector2.Distance(npc.Center, projectile.Center);
                    if (distance < MaximumDetectionDistance && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                    {
                        target = npc;
                        projectile.alpha = 0;
                        projectile.timeLeft = 600;
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

            if (target == null)
                return;

            if (projectile.ai[0] == 0f)
            {                    
                attackFrames = true;
                normalFrames = false;
                projectile.direction = 1;
                if (target.position.X - projectile.Center.X < 0f)
                {
                    projectile.spriteDirection = (projectile.direction = -1);
                }
                projectile.spriteDirection = projectile.direction;
                if (projectile.ai[1] == 0f)
                {
                    projectile.ai[1] = 1f;
                    Vector2 shootVel = target.position - projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("NPCStandFists"), MarineBiologist.standDamage, 1f, Main.myPlayer, 0, 0);
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

        public override void Kill(int timeLeft)
        {
            SPActive = false;
        }

        public virtual void SelectFrame()
        {
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                if (projectile.frameCounter >= 6)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                    if (projectile.frame >= 4)
                    {
                        projectile.frame = 2;
                    }
                }
            }
            if (normalFrames)
            {
                attackFrames = false;
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