using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace JoJoStands.NPCs.Enemies
{
    public class ChimeraBird : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.width = 46;
            NPC.height = 42;
            NPC.defense = 6;
            NPC.lifeMax = 140;
            NPC.damage = 17;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 1.3f;
            NPC.chaseable = true;
            NPC.noGravity = true;
            NPC.aiStyle = 0;
            NPC.value = 2 * 100;
        }

        //NPC.ai[0] = state (1 = Walking; 2 = Attacking)
        //NPC.ai[1] = jump cooldown
        //NPC.ai[2] = whether or not it's burning to death

        private const float AccelerationVelocity = 0.24f;

        private bool holdingZombieMouse = true;
        private NPC heldMouse = null;
        private int sinTimer = 0;

        public override void AI()
        {
            NPC.AddBuff(ModContent.BuffType<Vampire>(), 2);
            if (NPC.HasBuff(ModContent.BuffType<Sunburn>()))
            {
                NPC.defense = 0;
                NPC.damage = 0;
                for (int i = 0; i < 9; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, Main.rand.NextFloat(-0.6f, 0.6f + 1f), Main.rand.NextFloat(-0.6f, 1f));
                }
                NPC.life = 0;
                NPC.checkDead();
            }

            Player target = Main.player[NPC.target];
            if (target.dead || NPC.target == -1)
            {
                NPC.TargetClosest();
            }

            if (sinTimer < 360)
                sinTimer += 4;
            else
                sinTimer = 0;

            NPC.ai[0] = 1f;
            if (holdingZombieMouse)
            {
                if (heldMouse == null)
                {
                    heldMouse = Main.npc[NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<ZombieRat>())];
                }
                else
                {
                    heldMouse.position = NPC.Center + new Vector2(0f, 10f);
                    heldMouse.spriteDirection = heldMouse.direction = NPC.direction;
                    heldMouse.immortal = true;
                }

                if (NPC.position.Y > target.position.Y - 25f + ((float)Math.Sin(sinTimer) * 4f))
                {
                    NPC.velocity.Y -= AccelerationVelocity;
                }
                else
                {
                    NPC.velocity.Y += AccelerationVelocity;
                }

                if (Math.Abs(target.position.X - NPC.position.X) <= 4 * 16f)
                {
                    holdingZombieMouse = false;
                    heldMouse.immortal = false;
                    heldMouse.ai[2] = 1f;
                    heldMouse = null;
                }
            }
            else
            {
                if (NPC.position.Y > target.position.Y + ((float)Math.Sin(sinTimer) * 1.8f))
                {
                    NPC.velocity.Y -= AccelerationVelocity;
                }
                else
                {
                    NPC.velocity.Y += AccelerationVelocity;
                }
            }

            if (NPC.velocity.Y > 2f)
            {
                NPC.velocity.Y = 2f;
            }
            if (NPC.velocity.Y < -2f)
            {
                NPC.velocity.Y = -2f;
            }

            int velocityDirection;
            if (target.position.X > NPC.position.X)
            {
                velocityDirection = 1;
            }
            else
            {
                velocityDirection = -1;
            }

            if (Math.Abs(NPC.velocity.X) <= 5f)
            {
                NPC.velocity.X += AccelerationVelocity * velocityDirection;
            }

            if (NPC.velocity.X > 0)
            {
                NPC.direction = 1;
            }
            else
            {
                NPC.direction = -1;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (NPC.life < NPC.lifeMax)
            {
                int lifeStealAmount = damage / 4;
                NPC.life += lifeStealAmount;
            }
            if (NPC.life > NPC.lifeMax)
            {
                NPC.life = NPC.lifeMax;
            }

            if (Main.rand.Next(0, 101) <= 20)
            {
                target.AddBuff(BuffID.Bleeding, 3 * 60);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit)
        {
            if (NPC.life < NPC.lifeMax)
            {
                int lifeStealAmount = damage / 4;
                NPC.life += lifeStealAmount;
            }
            if (NPC.life > NPC.lifeMax)
            {
                NPC.life = NPC.lifeMax;
            }
            target.AddBuff(BuffID.Bleeding, 180);
        }

        public override bool CheckDead()
        {
            if (heldMouse != null)
            {
                heldMouse.immortal = false;
                heldMouse.life = 0;
                heldMouse.checkDead();
                heldMouse = null;
            }
            return true;
        }

        private int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 42;
            NPC.spriteDirection = NPC.direction;
            if (NPC.ai[0] == 0f)
            {
                frame = 0;
            }
            else if (NPC.ai[0] == 1f)
            {
                NPC.frameCounter += Math.Abs(NPC.velocity.X);
                if (NPC.frameCounter >= 10)
                {
                    frame++;
                    NPC.frameCounter = 0;

                    if (frame >= Main.npcFrameCount[NPC.type])
                    {
                        frame = 1;
                    }
                }
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float chance = 0f;
            if (JoJoStandsWorld.VampiricNight)
                chance = SpawnCondition.OverworldNightMonster.Chance;

            return chance;
        }
    }
}