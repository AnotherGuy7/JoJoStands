using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Buffs.Debuffs;
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
            Main.npcFrameCount[NPC.type] = 9;
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 28;
            NPC.defense = 3;
            NPC.lifeMax = 35;
            NPC.damage = 12;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 1.8f;
            NPC.chaseable = true;
            NPC.noGravity = false;
            NPC.aiStyle = 0;
            NPC.value = 70;
        }

        //NPC.ai[0] = state (1 = Falling; 2 = Walking)
        //NPC.ai[1] = jump cooldown
        //NPC.ai[2] = whether or not it's being held by the bird

        private const float AccelerationVelocity = 0.31f;

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

            if (NPC.ai[2] == 0f)
            {
                return;
            }

            Player target = Main.player[NPC.target];
            if (target.dead || NPC.target == -1)
            {
                NPC.TargetClosest();
            }

            if (NPC.ai[1] > 0)
            {
                NPC.ai[1] -= 1;
            }
            if (NPC.velocity.Y < 3f)
            {
                NPC.velocity.Y += 0.05f;
            }

            if (target.position.X > NPC.position.X)
            {
                NPC.direction = 1;
            }
            else
            {
                NPC.direction = -1;
            }

            if (Math.Abs(NPC.velocity.X) <= 6f)
            {
                NPC.velocity.X += AccelerationVelocity * NPC.direction;
            }

            if (WorldGen.SolidOrSlopedTile((int)(NPC.position.X / 16) + (int)Math.Ceiling(NPC.width / 16f) + 1 * NPC.direction, (int)(NPC.position.Y / 16f) + (int)Math.Ceiling(NPC.height / 16f) - 1) && NPC.ai[1] <= 0f)
            {
                NPC.velocity.Y = -6f;
                NPC.frameCounter = -40;     //This is to delay animations
                NPC.ai[1] = 60f;
            }
        }

        private int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[2] == 0f)
            {
                return;
            }

            frameHeight = 28;
            NPC.spriteDirection = -NPC.direction;
            if (NPC.ai[2] == 0f)
            {
                frame = 0;
            }
            else if (NPC.ai[2] == 1f && NPC.ai[1] <= 0f)
            {
                NPC.frameCounter += Math.Abs(NPC.velocity.X);
                if (NPC.frameCounter >= 4)
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
            return 0f;
        }
    }
}