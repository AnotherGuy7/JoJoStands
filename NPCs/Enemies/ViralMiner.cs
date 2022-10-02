using JoJoStands.Items.Tiles;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace JoJoStands.NPCs.Enemies
{
    public class ViralMiner : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4; //this defines how many frames the NPC sprite sheet has
        }

        public override void SetDefaults()
        {
            NPC.width = 38; //the NPC sprite width
            NPC.height = 48;  //the NPC sprite height
            NPC.defense = 13;  //the NPC defense
            NPC.lifeMax = 80;  // the NPC life
            NPC.HitSound = SoundID.NPCHit1;  //the NPC sound when is hit
            NPC.DeathSound = SoundID.NPCDeath1;  //the NPC sound when he dies
            NPC.knockBackResist = 1f;  //the NPC knockback resistance
            NPC.chaseable = true;       //whether or not minions can chase this NPC
            NPC.damage = 28;       //the damage the NPC does
            NPC.aiStyle = 0;        //no AI, to run void AI()
            NPC.noGravity = false;
            NPC.value = 8 * 100;
        }

        //NPC.ai[0] = timer before the NPC shoots
        //NPC.ai[1] = jump timer
        //NPC.ai[2] = tile detection change

        public override void AI()
        {
            Player target = Main.player[NPC.target];
            if (target.dead || NPC.target == -1)
            {
                NPC.TargetClosest();
            }
            if (NPC.velocity.Y < 3f)
            {
                NPC.velocity.Y += 0.05f;
            }
            if (NPC.ai[1] > 0f)
            {
                NPC.ai[1]--;
            }
            if (NPC.Distance(target.Center) <= 200f)
            {
                NPC.ai[0]++;
                NPC.velocity.X = 0f;
            }
            else
            {
                NPC.ai[0] = 0f;
                if (NPC.position.X > target.position.X + 200f)
                {
                    NPC.velocity.X = -1f;
                }
                if (NPC.position.X < target.position.X - 200f)
                {
                    NPC.velocity.X = 1f;
                }
                if (WorldGen.SolidOrSlopedTile((int)(NPC.position.X / 16) + (int)NPC.ai[2] * NPC.direction, (int)(NPC.position.Y / 16f) + 2) && NPC.ai[1] <= 0f)
                {
                    NPC.velocity.Y = -6f;
                    NPC.ai[1] += 40f;
                }
            }
            //Main.NewText((int)((NPC.position.X / 16) + NPC.ai[2] * NPC.direction) + "; " + (int)((NPC.position.Y / 16f) + 2) + "; " + (int)(Main.MouseWorld.X / 16f) + "; " + (Main.MouseWorld.Y / 16f));     Testing stuff to see where detection is and where it needs to be
            if (NPC.justHit)
            {
                NPC.ai[0] = 0f;
            }
            if (NPC.ai[0] >= 90f)
            {
                NPC.ai[0] = 0f;
                Vector2 shootVel = target.Center - NPC.Center;
                if (shootVel == Vector2.Zero)
                    shootVel = new Vector2(0f, 1f);

                shootVel.Normalize();
                shootVel *= 10f;
                int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X + (4f * NPC.direction), NPC.Center.Y - 5f, shootVel.X, shootVel.Y, ModContent.ProjectileType<MinerLightning>(), NPC.damage, 2f);
                Main.projectile[proj].netUpdate = true;
                NPC.netUpdate = true;
            }
            if (target.position.X > NPC.position.X)
            {
                NPC.direction = 1;
                NPC.ai[2] = 3f;
            }
            if (target.position.X < NPC.position.X)
            {
                NPC.direction = -1;
                NPC.ai[2] = 1f;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ViralMeteorite>(), 2, 1, 6));
        }

        private int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 48;
            NPC.spriteDirection = NPC.direction;
            if (NPC.velocity != Vector2.Zero)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 10)
                {
                    frame += 1;
                    NPC.frameCounter = 0;
                }
                if (frame >= 3)
                {
                    frame = 0;
                }
            }
            if (NPC.ai[0] != 0f)
            {
                frame = 3;
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<MyPlayer>().ZoneViralMeteorite)
            {
                return SpawnCondition.OverworldNightMonster.Chance * 2f;
            }
            else
            {
                return 0f;
            }
        }
    }
}