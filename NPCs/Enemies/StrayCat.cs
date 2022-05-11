using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class StrayCat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6; //this defines how many frames the NPC sprite sheet has
        }

        public override void SetDefaults()
        {
            NPC.width = 14; //the NPC sprite width
            NPC.height = 14;  //the NPC sprite height
            NPC.defense = 9;  //the NPC defense
            NPC.lifeMax = 60;  // the NPC life
            NPC.HitSound = SoundID.NPCHit1;  //the NPC sound when is hit
            NPC.DeathSound = SoundID.NPCDeath1;  //the NPC sound when he dies
            NPC.knockBackResist = 0f;  //the NPC knockback resistance
            NPC.chaseable = true;       //whether or not minions can chase this NPC
            NPC.damage = 21;       //the damage the NPC does
            NPC.aiStyle = 0;        //no AI, to run void AI()
            NPC.value = Item.buyPrice(0, 0, 6, 60);
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
                NPC.netUpdate = true;
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
                    int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, shootVel.X, shootVel.Y, ModContent.ProjectileType<AirBubble>(), NPC.damage, 1f);
                    Main.projectile[proj].netUpdate = true;
                    NPC.netUpdate = true;
                    NPC.ai[2] = 2f;
                }
            }
            if (target.position.X > NPC.position.X)
            {
                NPC.direction = -1;
                NPC.netUpdate = true;
            }
            if (target.position.X < NPC.position.X)
            {
                NPC.direction = 1;
                NPC.netUpdate = true;
            }
        }

        private int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 14;
            NPC.spriteDirection = NPC.direction;
            if (NPC.ai[1] == 1f)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 20)
                {
                    frame += 1;
                    NPC.frameCounter = 0;
                }
                if (NPC.ai[2] == 0f && frame == 4)
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
            if (!NPC.downedPlantBoss && !spawnInfo.Player.ZoneBeach && !spawnInfo.Player.ZoneDesert && !spawnInfo.Player.ZoneDungeon && spawnInfo.Player.ZoneOverworldHeight)
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