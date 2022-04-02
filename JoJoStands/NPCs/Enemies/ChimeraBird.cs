using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class ChimeraBird : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.width = 46;
            npc.height = 42;
            npc.defense = 6;
            npc.lifeMax = 140;
            npc.damage = 17;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 1.3f;
            npc.chaseable = true;
            npc.noGravity = true;
            npc.aiStyle = 0;
        }

        //npc.ai[0] = state (1 = Walking; 2 = Attacking)
        //npc.ai[1] = jump cooldown
        //npc.ai[2] = whether or not it's burning to death

        private const float AccelerationVelocity = 0.24f;

        private bool holdingZombieMouse = true;
        private NPC heldMouse = null;
        private int sinTimer = 0;

        public override void AI()
        {
            npc.AddBuff(mod.BuffType("Vampire"), 2);
            if (npc.HasBuff(mod.BuffType("Sunburn")))
            {
                npc.defense = 0;
                npc.damage = 0;
                for (int i = 0; i < 9; i++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.Smoke, Main.rand.NextFloat(-0.6f, 0.6f + 1f), Main.rand.NextFloat(-0.6f, 1f));
                }
                npc.life = 0;
                npc.checkDead();
            }

            Player target = Main.player[npc.target];
            if (target.dead || npc.target == -1)
            {
                npc.TargetClosest();
            }

            if (sinTimer < 360)
                sinTimer += 4;
            else
                sinTimer = 0;

            npc.ai[0] = 1f;
            if (holdingZombieMouse)
            {
                if (heldMouse == null)
                {
                    heldMouse = Main.npc[NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("ZombieRat"))];
                }
                else
                {
                    heldMouse.position = npc.Center + new Vector2(0f, 10f);
                    heldMouse.spriteDirection = heldMouse.direction = npc.direction;
                    heldMouse.immortal = true;
                }

                if (npc.position.Y > target.position.Y - 25f + ((float)Math.Sin(sinTimer) * 4f))
                {
                    npc.velocity.Y -= AccelerationVelocity;
                }
                else
                {
                    npc.velocity.Y += AccelerationVelocity;
                }

                if (Math.Abs(target.position.X - npc.position.X) <= 4 * 16f)
                {
                    holdingZombieMouse = false;
                    heldMouse.immortal = false;
                    heldMouse.ai[2] = 1f;
                    heldMouse = null;
                }
            }
            else
            {
                if (npc.position.Y > target.position.Y + ((float)Math.Sin(sinTimer) * 1.8f))
                {
                    npc.velocity.Y -= AccelerationVelocity;
                }
                else
                {
                    npc.velocity.Y += AccelerationVelocity;
                }
            }

            if (npc.velocity.Y > 2f)
            {
                npc.velocity.Y = 2f;
            }
            if (npc.velocity.Y < -2f)
            {
                npc.velocity.Y = -2f;
            }

            int velocityDirection;
            if (target.position.X > npc.position.X)
            {
                velocityDirection = 1;
            }
            else
            {
                velocityDirection = -1;
            }

            if (Math.Abs(npc.velocity.X) <= 5f)
            {
                npc.velocity.X += AccelerationVelocity * velocityDirection;
            }

            if (npc.velocity.X > 0)
            {
                npc.direction = 1;
            }
            else
            {
                npc.direction = -1;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (npc.life < npc.lifeMax)
            {
                int lifeStealAmount = damage / 4;
                npc.life += lifeStealAmount;
            }
            if (npc.life > npc.lifeMax)
            {
                npc.life = npc.lifeMax;
            }

            if (Main.rand.Next(0, 101) <= 20)
            {
                target.AddBuff(BuffID.Bleeding, 3 * 60);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (npc.life < npc.lifeMax)
            {
                int lifeStealAmount = damage / 4;
                npc.life += lifeStealAmount;
            }
            if (npc.life > npc.lifeMax)
            {
                npc.life = npc.lifeMax;
            }
            target.AddBuff(BuffID.Bleeding, 180);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.SilverCoin, Main.rand.Next(0, 1 + 1));
            Item.NewItem(npc.getRect(), ItemID.CopperCoin, Main.rand.Next(0, 99 + 1));
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
            npc.spriteDirection = npc.direction;
            if (npc.ai[0] == 0f)
            {
                frame = 0;
            }
            else if (npc.ai[0] == 1f)
            {
                npc.frameCounter += Math.Abs(npc.velocity.X);
                if (npc.frameCounter >= 10)
                {
                    frame++;
                    npc.frameCounter = 0;

                    if (frame >= Main.npcFrameCount[npc.type])
                    {
                        frame = 1;
                    }
                }
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float chance = 0f;
            if (JoJoStandsWorld.VampiricNight)
            {
                chance = SpawnCondition.OverworldNightMonster.Chance;
            }
            return chance;
        }
    }
}