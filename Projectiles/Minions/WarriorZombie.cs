using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles.Minions
{
    public class WarriorZombie : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.timeLeft = 90 * 60;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 10;
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
                    while (!WorldGen.SolidTile((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f) - 2) || WorldGen.SolidTile((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f) - 3))
                    {
                        Projectile.position.Y += 1f;
                        if (Projectile.position.Y / 16f > Main.maxTilesY)
                        {
                            break;
                        }
                    }
                    checkedForGround = true;
                }
                Projectile.position.Y -= 0.5f;
                Projectile.alpha = (int)(255f * (spawnTimer / 120f));
                if (dirtSpawnPosition == Vector2.Zero)
                {
                    dirtSpawnPosition = Projectile.Center - new Vector2(0f, Projectile.height);
                }
                for (int d = 0; d < 11; d++)
                {
                    int dustIndex = Dust.NewDust(dirtSpawnPosition, Projectile.width, 6, DustID.Dirt);
                    Main.dust[dustIndex].noGravity = true;
                }
                return;
            }

            if (jumpRestTimer > 0)
                jumpRestTimer--;
            if (Projectile.velocity.Y < 5f)
                Projectile.velocity.Y += 0.2f;

            Vector3 lightLevel = Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16).ToVector3();
            if (lightLevel.Length() > 1.3f && Main.dayTime && Main.tile[(int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16].WallType == 0)
            {
                Projectile.Kill();
            }

            NPC target = null;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (npc.lifeMax > 5 && !npc.friendly && !npc.townNPC && !npc.hide && Projectile.Distance(npc.Center) <= DetectionDistance)
                    {
                        target = npc;
                        break;
                    }
                }
            }

            if (target != null)
            {
                if (Projectile.position.X > target.position.X)
                {
                    Projectile.direction = -1;
                }
                else
                {
                    Projectile.direction = 1;
                }
                if (Math.Abs(Projectile.velocity.X) < 3.5f)
                {
                    Projectile.velocity.X += 0.2f * Projectile.direction;
                }
                if (WorldGen.SolidTile((int)(Projectile.Center.X / 16f) + Projectile.direction, (int)(Projectile.Center.Y / 16f)) && jumpRestTimer <= 0)
                {
                    jumpRestTimer += 40;
                    Projectile.velocity.Y = -8f;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                if (Math.Abs(Projectile.velocity.X) > 0f)
                {
                    Projectile.velocity.X *= 0.6718057f;
                }
            }

            Projectile.spriteDirection = Projectile.direction;
            if (Math.Abs(Projectile.velocity.X) >= 0.2f)
            {
                Projectile.frameCounter += (int)Math.Ceiling(Projectile.velocity.X);
                if (Projectile.frameCounter >= 22f)
                {
                    if (Projectile.frame < WalkingFramesMinimum || Projectile.frame > WalkingFramesMaximum)
                    {
                        Projectile.frame = WalkingFramesMinimum;
                    }
                }
            }
            else
            {
                Projectile.frame = IdleFrame;
            }
            if (!WorldGen.SolidTile((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f) + 2))
            {
                Projectile.frame = JumpingFrame;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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
                Dust.NewDust(dirtSpawnPosition + angle.ToRotationVector2(), Projectile.width, Projectile.height, DustID.Smoke);
            }
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}