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
            Main.npcFrameCount[npc.type] = 6; //this defines how many frames the npc sprite sheet has
        }

        public override void SetDefaults()
        {
            npc.width = 14; //the npc sprite width
            npc.height = 14;  //the npc sprite height
            npc.defense = 9;  //the npc defense
            npc.lifeMax = 60;  // the npc life
            npc.HitSound = SoundID.NPCHit1;  //the npc sound when is hit
            npc.DeathSound = SoundID.NPCDeath1;  //the npc sound when he dies
            npc.knockBackResist = 0f;  //the npc knockback resistance
            npc.chaseable = true;       //whether or not minions can chase this npc
            npc.damage = 21;       //the damage the npc does
            npc.aiStyle = 0;        //no AI, to run void AI()
            npc.value = Item.buyPrice(0, 0, 6, 60);
        }

        //npc.ai[0] = timer before the NPC shoots
        //npc.ai[1] = NPC state (0 = idle; 1 = shooting; 2 = defending)
        //npc.ai[2] = cooldown befoer Stray Cat is set to shoot again
        //npc.ai[3] = 

        public override void AI()
        {
            Player target = Main.player[npc.target];
            npc.velocity.Y = 2f;
            if (npc.Distance(target.position) <= 300f && npc.ai[1] == 0f)
            {
                npc.ai[0]++;
            }
            if (npc.ai[0] > 0f && npc.ai[1] == 1f)
            {
                npc.ai[0]--;
            }
            if (npc.ai[0] >= 210f)
            {
                npc.ai[0] = 209f;
                npc.ai[1] = 1f;
            }
            if (npc.ai[1] == 1f)
            {
                if (npc.ai[2] == 1f)
                {
                    Vector2 shootVel = target.Center - npc.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= 2f;
                    int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("AirBubble"), npc.damage, 1f);
                    Main.projectile[proj].netUpdate = true;
                    npc.netUpdate = true;
                    npc.ai[2] = 2f;
                }
            }
            if (target.position.X > npc.position.X)
            {
                npc.direction = -1;
                npc.netUpdate = true;
            }
            if (target.position.X < npc.position.X)
            {
                npc.direction = 1;
                npc.netUpdate = true;
            }
        }

        private int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 14;
            npc.spriteDirection = npc.direction;
            if (npc.ai[1] == 1f)
            {
                npc.frameCounter++;
                if (npc.frameCounter >= 20)
                {
                    frame += 1;
                    npc.frameCounter = 0;
                }
                if (npc.ai[2] == 0f && frame == 4)
                {
                    npc.ai[2] = 1f;
                }
                if (frame >= 6)
                {
                    frame = 0;
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                }
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!NPC.downedPlantBoss && !spawnInfo.player.ZoneBeach && !spawnInfo.player.ZoneDesert && !spawnInfo.player.ZoneDungeon && spawnInfo.player.ZoneOverworldHeight)
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