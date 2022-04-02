using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class WangChan : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 22;
        }

        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 48;
            npc.defense = 24;
            npc.lifeMax = 150;
            npc.damage = 60;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 1f;
            npc.chaseable = true;
            npc.noGravity = false;
            npc.daybreak = true;
            npc.aiStyle = 3;
            aiType = 199;
        }

        private const int IdleFrame = 0;
        private const int WalkFramesMinimum = 1;
        private const int WalkFramesMaximum = 14;
        private const int JumpingFrame = 15;
        private const int SwingFramesMinimum = 16;
        private const int SwingFramesMaximum = 18;
        private const int PoisonThrowFramesMinimum = 19;
        private const int PoisonThrowFramesMaximum = 21;

        private int frame = 0;
        private int runCounter = 0;
        private int poisonThrowTimer = 0;
        private int poisonCooldown = 0;
        private int expertDamageMultiplier = 1;
        private int jumpCooldown = 0;
        private bool poisonThrow = false;

        public override void AI()
        {
            Player target = Main.player[npc.target];
            if (runCounter > 0)
            {
                runCounter -= 1;
            }
            if (target.dead || npc.target == -1)
            {
                npc.TargetClosest();
            }
            if (Main.expertMode)
            {
                expertDamageMultiplier = 2;
            }

            npc.AddBuff(mod.BuffType("Vampire"), 2);
            if (npc.HasBuff(mod.BuffType("Sunburn")))
            {
                npc.defense = 0;
                npc.damage = 20 * expertDamageMultiplier;
            }
            else
            {
                npc.defense = 24;
                npc.damage = 60 * expertDamageMultiplier;
            }

            if (npc.velocity.X > 0)
            {
                npc.spriteDirection = 1;
            }
            if (npc.velocity.X < 0)
            {
                npc.spriteDirection = -1;
            }

            if (poisonCooldown > 0)
            {
                poisonCooldown -= 1;
            }

            if (npc.life > npc.lifeMax)
            {
                npc.life = npc.lifeMax;
            }

            if (!poisonThrow)
            {
                aiType = 199;
                npc.aiStyle = 3;
                if (npc.collideY && jumpCooldown == 0 && npc.velocity.Y == 0)      //jump jump jump
                {
                    jumpCooldown = 45;
                    npc.velocity.Y -= 9f;
                }
                if (npc.velocity.Y < 3f)
                {
                    npc.velocity.Y += 0.15f;
                }
                if (jumpCooldown > 0)
                {
                    jumpCooldown--;
                }
            }

            if (runCounter == 0)
            {
                if (npc.position.X >= target.position.X)
                {
                    if (npc.position.X - 50 >= target.position.X)
                    {
                        npc.direction = -1;
                        npc.velocity.X = -3f;
                    }
                }
                if (npc.position.X < target.position.X)
                {
                    if (npc.position.X + 50 < target.position.X)
                    {
                        npc.direction = 1;
                        npc.velocity.X = 3f;
                    }
                }
            }
            if (runCounter != 0)
            {
                if (npc.position.X >= target.position.X)
                {
                    npc.direction = 1;
                    npc.velocity.X = 3f;
                }
                if (npc.position.X < target.position.X)
                {
                    npc.direction = -1;
                    npc.velocity.X = -3f;
                }
            }

            if (npc.Distance(target.Center) >= 300f && runCounter > 0)
            {
                runCounter = 0;
                poisonCooldown = 0;
            }

            if (!npc.noTileCollide && npc.Distance(target.Center) <= 300f && poisonCooldown <= 0 && !target.dead)       //poison
            {
                poisonThrow = true;
            }
            if (poisonThrow)
            {
                npc.aiStyle = 0;
                npc.velocity.X = 0;
                poisonThrowTimer += 1;
                if (poisonThrowTimer == 45)
                {
                    Vector2 shootVel = target.Center - npc.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 0f);
                    }
                    shootVel.Normalize();
                    shootVel *= 10f;
                    float rotationInRadians = 0f;
                    if (target.position.X < npc.position.X)
                    {
                        rotationInRadians = 30f;
                    }
                    if (target.position.X > npc.position.X)
                    {
                        rotationInRadians = -30f;
                    }
                    float rotation = MathHelper.ToRadians(rotationInRadians);
                    Vector2 newSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, 1) * 1f);
                    int proj = Projectile.NewProjectile(npc.Center, newSpeed, mod.ProjectileType("AcidVenomFlask"), 21 * expertDamageMultiplier, 1f);
                    Main.projectile[proj].netUpdate = true;
                    npc.netUpdate = true;
                }
                if (poisonThrowTimer >= 90)
                {
                    poisonThrow = false;
                    runCounter = 0;
                    poisonThrowTimer = 0;
                    poisonCooldown = 800;
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (npc.life < npc.lifeMax)
            {
                int lifeStealAmount = damage / 2;
                npc.life += lifeStealAmount;
            }
            if (!poisonThrow && runCounter == 0)
            {
                runCounter += 200;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (npc.life < npc.lifeMax)
            {
                int lifeStealAmount = damage / 2;
                npc.life += lifeStealAmount;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 56;
            npc.frameCounter++;
            float velX = Math.Abs(npc.velocity.X);
            if (velX > 0f)
            {
                npc.frameCounter += velX;
                if (npc.frameCounter >= 15)
                {
                    npc.frameCounter = 0;
                    frame += 1;
                    if (frame > WalkFramesMaximum)
                    {
                        frame = WalkFramesMinimum;
                    }
                }
            }
            else
            {
                frame = IdleFrame;
            }
            if (npc.velocity.Y != 0f)
            {
                frame = JumpingFrame;
            }
            if (poisonThrow)
            {
                int frameDifference = PoisonThrowFramesMaximum - PoisonThrowFramesMinimum;
                int timerFraction = 90 / frameDifference;
                int frameAddition = frameDifference * (poisonThrowTimer / timerFraction);
                frame = PoisonThrowFramesMinimum + frameAddition;
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float chance = 0f;
            if (JoJoStandsWorld.VampiricNight)
            {
                chance = 0.01f;
            }
            return chance;
        }
    }
}