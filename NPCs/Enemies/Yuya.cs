using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.NPCs.Enemies
{
    public class Yuya : ModNPC
    {
        public int standcounter = 0;
        public int frame = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 16; //this defines how many frames the NPC sprite sheet has
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 32;
            NPC.defense = 5;
            NPC.lifeMax = 110;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 1f;
            NPC.chaseable = true;
            NPC.damage = 5;
            NPC.aiStyle = 0;
            NPC.value = 3 * 100 * 100;
            NPC.rarity = 2;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("An ex-gang member who suffered an accident and lost his old life. He was saved by another Stand User and continued on a different path.")
            });
        }

        //NPC.ai[2] = is stand alive
        //NPC.ai[3] = is user alive

        public override bool CheckActive()
        {
            NPC.ai[3] = 1f;
            return base.CheckActive();
        }

        public override bool CheckDead()
        {
            NPC.ai[3] = 0f;
            NPC.ai[2] = 0f;
            return base.CheckDead();
        }

        public override void AI()       //a really trashy custom ai...
        {
            NPC.spriteDirection = NPC.direction;
            if (NPC.active)
            {
                NPC.ai[0]++;
            }
            if (NPC.ai[0] == 1f && NPC.ai[2] == 0f)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<HighwayStar>(), 0, 0f, 0f, NPC.whoAmI, 0f, NPC.target);
            }
            if (NPC.ai[2] == 0f)
            {
                standcounter++;
            }
            if (standcounter == 180)      //once the stand is dead, spawn another one but only one more time, which is why I never set standcounter to restart
            {
                NPC.ai[0] = 0f;
            }
            if (NPC.justHit)        //run the other way when hit
            {
                if (NPC.direction == 1)
                {
                    NPC.ai[1] = 1f;
                }
                if (NPC.direction == -1)
                {
                    NPC.spriteDirection = 1;
                    NPC.direction = 1;
                    NPC.ai[1] = 2f;
                }
            }
            if (NPC.ai[1] == 1f)
            {
                NPC.spriteDirection = -1;
                NPC.direction = -1;
                NPC.velocity.X = -2f;
            }
            if (NPC.ai[1] == 2f)
            {
                NPC.spriteDirection = 1;
                NPC.direction = 1;
                NPC.velocity.X = 2f;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 44;
            NPC.spriteDirection = NPC.direction;
            NPC.frameCounter++;
            if (NPC.frameCounter >= 10)
            {
                NPC.frameCounter = 0;
            }
            if (NPC.life == NPC.lifeMax)        //stand still frames
            {
                frame = 1;
            }
            if (NPC.ai[1] == 1f || NPC.ai[1] == 2f)        //run if he got hit frames
            {
                if (NPC.frameCounter <= 10)
                {
                    frame += 1;
                }
                if (frame <= 1)
                {
                    frame = 2;
                }
                if (frame >= 16)
                {
                    frame = 2;
                }
                NPC.frame.Y = frame * frameHeight;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode)
                return SpawnCondition.OverworldNightMonster.Chance * 0.07f;

            return 0f;
        }
    }
}