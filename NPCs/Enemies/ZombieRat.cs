using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class ZombieRat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 12;
            npc.defense = 3;
            npc.lifeMax = 35;
            npc.damage = 12;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 1.8f;
            npc.chaseable = true;
            npc.noGravity = false;
            npc.aiStyle = 0;
        }

        //npc.ai[0] = state (1 = Falling; 2 = Walking)
        //npc.ai[1] = jump cooldown
        //npc.ai[2] = whether or not it's being held by the bird

        private const float AccelerationVelocity = 0.31f;

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

            if (npc.ai[2] == 0f)
            {
                return;
            }

            Player target = Main.player[npc.target];
            if (target.dead || npc.target == -1)
            {
                npc.TargetClosest();
            }

            if (npc.ai[1] > 0)
            {
                npc.ai[1] -= 1;
            }
            if (npc.velocity.Y < 3f)
            {
                npc.velocity.Y += 0.05f;
            }

            if (target.position.X > npc.position.X)
            {
                npc.direction = 1;
            }
            else
            {
                npc.direction = -1;
            }

            if (Math.Abs(npc.velocity.X) <= 6f)
            {
                npc.velocity.X += AccelerationVelocity * npc.direction;
            }

            if (WorldGen.SolidOrSlopedTile((int)(npc.position.X / 16) + (int)Math.Ceiling(npc.width / 16f) + 1, (int)(npc.position.Y / 16f) + (int)Math.Ceiling(npc.height / 16f) - 1) && npc.ai[1] <= 0f)
            {
                npc.velocity.Y = -6f;
                npc.frameCounter = -40;     //This is to delay animations
                npc.ai[1] = 60f;
            }
    }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.CopperCoin, Main.rand.Next(0, 99 + 1));
        }

        private int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[2] == 0f)
            {
                return;
            }

            frameHeight = 12;
            npc.spriteDirection = npc.direction;
            if (npc.ai[0] == 0f)
            {
                frame = 0;
            }
            else if (npc.ai[0] == 1f)
            {
                npc.frameCounter += Math.Abs(npc.velocity.X);
                if (npc.frameCounter >= 12)
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
            return 0f;
        }
    }
}