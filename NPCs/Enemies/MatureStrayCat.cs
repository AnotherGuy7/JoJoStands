using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class MatureStrayCat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6; //this defines how many frames the NPC sprite sheet has
        }

        public override void SetDefaults()
        {
            NPC.width = 16;
            NPC.height = 20;
            NPC.defense = 47;
            NPC.lifeMax = 400;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.chaseable = true;
            NPC.damage = 120;
            NPC.aiStyle = 0;
            NPC.value = Item.buyPrice(gold: 1, silver: 75);
            NPC.catchItem = (short)ModContent.ItemType<Items.StrayCat>();
            NPC.rarity = 2;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("A peculiar plant(?) that seems to react differently to different people, though hostile to most!")
            });
        }

        //NPC.ai[0] = timer before the NPC shoots
        //NPC.ai[1] = NPC state (0 = idle; 1 = shooting; 2 = defending)
        //NPC.ai[2] = cooldown befoer Stray Cat is set to shoot again
        //NPC.ai[3] = 

        public override void AI()
        {
            Player target = Main.player[NPC.target];
            NPC.velocity.Y = 2f;
            if (NPC.Distance(target.position) <= 300f && NPC.ai[1] == 0f)
            {
                NPC.ai[0]++;
            }
            if (NPC.ai[0] > 0f && NPC.ai[1] == 1f)
            {
                NPC.ai[0]--;
            }
            if (NPC.ai[0] >= 210f)
            {
                NPC.ai[0] = 209f;
                NPC.ai[1] = 1f;
            }
            if (NPC.ai[1] == 1f)
            {
                if (NPC.ai[2] == 1f)
                {
                    Vector2 shootVel = target.Center - NPC.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= 2f;
                    int projIndex = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, shootVel.X, shootVel.Y, ModContent.ProjectileType<AirBubble>(), NPC.damage, 1f);
                    Main.projectile[projIndex].netUpdate = true;
                    NPC.ai[2] = 2f;

                }
            }
            if (target.position.X > NPC.position.X)
            {
                NPC.direction = -1;
            }
            if (target.position.X < NPC.position.X)
            {
                NPC.direction = 1;
            }
        }

        private int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 20;
            NPC.spriteDirection = NPC.direction;
            if (NPC.ai[1] == 1f)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 20)
                {
                    frame += 1;
                    NPC.frameCounter = 0;
                }
                if (NPC.ai[2] == 0f && frame == 5)
                {
                    NPC.ai[2] = 1f;
                }
                if (frame >= 6)
                {
                    frame = 0;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                }
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedPlantBoss && !spawnInfo.Player.ZoneBeach && !spawnInfo.Player.ZoneDesert && !spawnInfo.Player.ZoneDungeon && spawnInfo.Player.ZoneOverworldHeight)
            {
                return 0.04f;
            }
            else
            {
                return 0f;
            }
        }
    }
}