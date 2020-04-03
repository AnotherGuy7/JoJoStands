using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.Minions
{  
    public class MatureStrayCatMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 16;
            projectile.height = 28;
            projectile.friendly = true;
            projectile.netImportant = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.timeLeft = 2;
            projectile.minionSlots = 0.5f;
            MyPlayer.stopimmune.Add(mod.ProjectileType(Name));
            drawOffsetX = 8;
            drawOriginOffsetY = 6;
            projectile.penetrate = -1;
            projectile.damage = 0;
        }

        public bool canShoot = false;
        public int shootCount = 0;

        public override void AI()       ////I really just ported over the Stray Cat NPC AI
        {
            SelectFrame();
            Player player = Main.player[projectile.owner];
            NPC target = null;
            projectile.damage = 0;
            if (shootCount > 0)
            {
                shootCount--;
            }
            if (target == null)
            {
                for (int k = 0; k < Main.maxNPCs; k++)
                {
                    NPC potentialTarget = Main.npc[k];
                    if (potentialTarget.active && !potentialTarget.dontTakeDamage && !potentialTarget.friendly && potentialTarget.lifeMax > 5 && potentialTarget.type != NPCID.TargetDummy && potentialTarget.type != NPCID.CultistTablet && !potentialTarget.townNPC && projectile.Distance(potentialTarget.Center) <= 400f)
                    {
                        target = potentialTarget;
                    }
                }
            }
            projectile.timeLeft = 2;
            if (player.HeldItem.type == mod.ItemType("StrayCat") && player.altFunctionUse == 2)
            {
                projectile.Kill();
            }
            projectile.velocity.Y = 2f;
            if (target != null)
            {
                if (projectile.ai[1] == 0f)
                {
                    projectile.ai[0]++;
                }
                if (projectile.ai[0] > 0f && projectile.ai[1] == 1f)
                {
                    projectile.ai[0]--;
                }
                if (projectile.ai[0] >= 210f)
                {
                    projectile.ai[0] = 209f;
                    projectile.ai[1] = 1f;
                }
                if (projectile.ai[1] == 1f)
                {
                    if (canShoot && shootCount <= 0)
                    {
                        shootCount += 40;
                        Vector2 shootVel = target.Center - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 2f;
                        int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("AirBubble"), 104, 1f, projectile.owner);
                        Main.projectile[proj].hostile = false;
                        Main.projectile[proj].friendly = true;
                        Main.projectile[proj].netUpdate = true;
                        canShoot = false;

                    }
                }
                if (target.position.X > projectile.position.X)
                {
                    projectile.direction = 1;
                }
                if (target.position.X < projectile.position.X)
                {
                    projectile.direction = -1;
                }
            }
            projectile.spriteDirection = projectile.direction;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public void SelectFrame()
        {
            projectile.spriteDirection = projectile.direction;
            if (projectile.ai[1] == 1f)
            {
                projectile.frameCounter++;
                if (projectile.frameCounter >= 20)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (!canShoot && projectile.frame == 5)
                {
                    canShoot = true;
                }
                if (projectile.frame >= 6)
                {
                    projectile.frame = 0;
                    projectile.ai[1] = 0f;
                    canShoot = false;
                }
            }
        }
    }
}