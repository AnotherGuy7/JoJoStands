using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Buffs.Debuffs;
using JoJoStands.Projectiles;
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
            Main.npcFrameCount[NPC.type] = 22;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 48;
            NPC.defense = 24;
            NPC.lifeMax = 150;
            NPC.damage = 60;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 1f;
            NPC.chaseable = true;
            NPC.noGravity = false;
            NPC.daybreak = true;
            NPC.aiStyle = 3;
            AIType = 199;
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
            Player target = Main.player[NPC.target];
            if (runCounter > 0)
            {
                runCounter -= 1;
            }
            if (target.dead || NPC.target == -1)
            {
                NPC.TargetClosest();
            }
            if (Main.expertMode)
            {
                expertDamageMultiplier = 2;
            }

            NPC.AddBuff(ModContent.BuffType<Vampire>(), 2);
            if (NPC.HasBuff(ModContent.BuffType<Sunburn>()))
            {
                NPC.defense = 0;
                NPC.damage = 20 * expertDamageMultiplier;
            }
            else
            {
                NPC.defense = 24;
                NPC.damage = 60 * expertDamageMultiplier;
            }

            if (NPC.velocity.X > 0)
            {
                NPC.spriteDirection = 1;
            }
            if (NPC.velocity.X < 0)
            {
                NPC.spriteDirection = -1;
            }

            if (poisonCooldown > 0)
            {
                poisonCooldown -= 1;
            }

            if (NPC.life > NPC.lifeMax)
            {
                NPC.life = NPC.lifeMax;
            }

            if (!poisonThrow)
            {
                AIType = 199;
                NPC.aiStyle = 3;
                if (NPC.collideY && jumpCooldown == 0 && NPC.velocity.Y == 0)      //jump jump jump
                {
                    jumpCooldown = 45;
                    NPC.velocity.Y -= 9f;
                }
                if (NPC.velocity.Y < 3f)
                {
                    NPC.velocity.Y += 0.15f;
                }
                if (jumpCooldown > 0)
                {
                    jumpCooldown--;
                }
            }

            if (runCounter == 0)
            {
                if (NPC.position.X >= target.position.X)
                {
                    if (NPC.position.X - 50 >= target.position.X)
                    {
                        NPC.direction = -1;
                        NPC.velocity.X = -3f;
                    }
                }
                if (NPC.position.X < target.position.X)
                {
                    if (NPC.position.X + 50 < target.position.X)
                    {
                        NPC.direction = 1;
                        NPC.velocity.X = 3f;
                    }
                }
            }
            if (runCounter != 0)
            {
                if (NPC.position.X >= target.position.X)
                {
                    NPC.direction = 1;
                    NPC.velocity.X = 3f;
                }
                if (NPC.position.X < target.position.X)
                {
                    NPC.direction = -1;
                    NPC.velocity.X = -3f;
                }
            }

            if (NPC.Distance(target.Center) >= 300f && runCounter > 0)
            {
                runCounter = 0;
                poisonCooldown = 0;
            }

            if (!NPC.noTileCollide && NPC.Distance(target.Center) <= 300f && poisonCooldown <= 0 && !target.dead)       //poison
            {
                poisonThrow = true;
            }
            if (poisonThrow)
            {
                NPC.aiStyle = 0;
                NPC.velocity.X = 0;
                poisonThrowTimer += 1;
                if (poisonThrowTimer == 45)
                {
                    Vector2 shootVel = target.Center - NPC.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 0f);
                    }
                    shootVel.Normalize();
                    shootVel *= 10f;
                    float rotationInRadians = 0f;
                    if (target.position.X < NPC.position.X)
                    {
                        rotationInRadians = 30f;
                    }
                    if (target.position.X > NPC.position.X)
                    {
                        rotationInRadians = -30f;
                    }
                    float rotation = MathHelper.ToRadians(rotationInRadians);
                    Vector2 newSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, 1) * 1f);
                    int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, newSpeed, ModContent.ProjectileType<AcidVenomFlask>(), 21 * expertDamageMultiplier, 1f);
                    Main.projectile[proj].netUpdate = true;
                    NPC.netUpdate = true;
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
            if (NPC.life < NPC.lifeMax)
            {
                int lifeStealAmount = damage / 2;
                NPC.life += lifeStealAmount;
            }
            if (!poisonThrow && runCounter == 0)
            {
                runCounter += 200;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (NPC.life < NPC.lifeMax)
            {
                int lifeStealAmount = damage / 2;
                NPC.life += lifeStealAmount;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 56;
            NPC.frameCounter++;
            float velX = Math.Abs(NPC.velocity.X);
            if (velX > 0f)
            {
                NPC.frameCounter += velX;
                if (NPC.frameCounter >= 15)
                {
                    NPC.frameCounter = 0;
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
            if (NPC.velocity.Y != 0f)
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
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float chance = 0f;
            if (JoJoStandsWorld.VampiricNight)
                chance = 0.01f;

            return chance;
        }
    }
}