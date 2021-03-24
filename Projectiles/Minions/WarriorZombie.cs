using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{
    public class WarriorZombie : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 34;
            projectile.friendly = true;
            projectile.timeLeft = 90 * 60;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 10;
        }

        private const int IdleFrame = 0;
        private const int WalkingFramesMinimum = 1;
        private const int WalkingFramesMaximum = 4;
        private const int JumpingFrame = 5;
        private const float DetectionDistance = 24f * 16f;

        private int spawnTimer = 0;
        private int jumpRestTimer = 0;
        private bool checkedForGround = false;
        private Vector2 dirtSpawnPosition;

        public override void AI()
        {
            if (spawnTimer < 120)
            {
                spawnTimer++;
                if (!checkedForGround)
                {
                    while (!WorldGen.SolidTile((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f) - 2) || WorldGen.SolidTile((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f) - 3))
                    {
                        projectile.position.Y += 1f;
                        if (projectile.position.Y / 16f > Main.maxTilesY)
                        {
                            break;
                        }
                    }
                    checkedForGround = true;
                }
                projectile.position.Y -= 0.5f;
                projectile.alpha = (int)(255f * (spawnTimer / 120f));
                if (dirtSpawnPosition == Vector2.Zero)
                {
                    dirtSpawnPosition = projectile.Center - new Vector2(0f, projectile.height);
                }
                for (int d = 0; d < 11; d++)
                {
                    int dustIndex = Dust.NewDust(dirtSpawnPosition, projectile.width, 6, DustID.Dirt);
                    Main.dust[dustIndex].noGravity = true;
                }
                return;
            }

            if (jumpRestTimer > 0)
                jumpRestTimer--;
            if (projectile.velocity.Y < 5f)
                projectile.velocity.Y += 0.2f;

            Vector3 lightLevel = Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16).ToVector3();
            if (lightLevel.Length() > 1.3f && Main.dayTime && Main.tile[(int)projectile.Center.X / 16, (int)projectile.Center.Y / 16].wall == 0)
            {
                projectile.Kill();
            }

            NPC target = null;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (npc.lifeMax > 5 && !npc.friendly && !npc.townNPC && !npc.hide && projectile.Distance(npc.Center) <= DetectionDistance)
                    {
                        target = npc;
                        break;
                    }
                }
            }

            if (target != null)
            {
                if (projectile.position.X > target.position.X)
                {
                    projectile.direction = -1;
                }
                else
                {
                    projectile.direction = 1;
                }
                if (Math.Abs(projectile.velocity.X) < 3.5f)
                {
                    projectile.velocity.X += 0.2f * projectile.direction;
                }
                if (WorldGen.SolidTile((int)(projectile.Center.X / 16f) + projectile.direction, (int)(projectile.Center.Y / 16f)) && jumpRestTimer <= 0)
                {
                    jumpRestTimer += 40;
                    projectile.velocity.Y = -8f;
                    projectile.netUpdate = true;
                }
            }
            else
            {
                if (Math.Abs(projectile.velocity.X) > 0f)
                {
                    projectile.velocity.X *= 0.6718057f;
                }
            }

            projectile.spriteDirection = projectile.direction;
            if (Math.Abs(projectile.velocity.X) >= 0.2f)
            {
                projectile.frameCounter += (int)Math.Ceiling(projectile.velocity.X);
                if (projectile.frameCounter >= 22f)
                {
                    if (projectile.frame < WalkingFramesMinimum || projectile.frame > WalkingFramesMaximum)
                    {
                        projectile.frame = WalkingFramesMinimum;
                    }
                }
            }
            else
            {
                projectile.frame = IdleFrame;
            }
            if (!WorldGen.SolidTile((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f) + 2))
            {
                projectile.frame = JumpingFrame;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(0, 101) <= 20)
            {
                target.AddBuff(BuffID.Bleeding, 5 * 60);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d < Main.rand.Next(3, 9); d++)
            {
                float angle = d;
                Dust.NewDust(dirtSpawnPosition + angle.ToRotationVector2(), projectile.width, projectile.height, DustID.Smoke);
            }
            Main.PlaySound(SoundID.NPCDeath1, projectile.position);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}