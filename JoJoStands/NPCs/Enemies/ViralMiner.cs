using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class ViralMiner : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4; //this defines how many frames the npc sprite sheet has
        }

        public override void SetDefaults()
        {
            npc.width = 40; //the npc sprite width
            npc.height = 48;  //the npc sprite height
            npc.defense = 13;  //the npc defense
            npc.lifeMax = 80;  // the npc life
            npc.HitSound = SoundID.NPCHit1;  //the npc sound when is hit
            npc.DeathSound = SoundID.NPCDeath1;  //the npc sound when he dies
            npc.knockBackResist = 1f;  //the npc knockback resistance
            npc.chaseable = true;       //whether or not minions can chase this npc
            npc.damage = 28;       //the damage the npc does
            npc.aiStyle = 0;        //no AI, to run void AI()
            npc.noGravity = false;
        }

        //npc.ai[0] = timer before the NPC shoots
        //npc.ai[1] = jump timer
        //npc.ai[2] = tile detection change

        public override void AI()
        {
            Player target = Main.player[npc.target];
            if (target.dead || npc.target == -1)
            {
                npc.TargetClosest();
            }
            if (npc.velocity.Y < 3f)
            {
                npc.velocity.Y += 0.05f;
            }
            if (npc.ai[1] > 0f)
            {
                npc.ai[1]--;
            }
            if (npc.Distance(target.Center) <= 200f)
            {
                npc.ai[0]++;
                npc.velocity.X = 0f;
            }
            else
            {
                npc.ai[0] = 0f;
                if (npc.position.X > target.position.X + 200f)
                {
                    npc.velocity.X = -1f;
                }
                if (npc.position.X < target.position.X - 200f)
                {
                    npc.velocity.X = 1f;
                }
                if (WorldGen.SolidOrSlopedTile((int)(npc.position.X / 16) + (int)npc.ai[2] * npc.direction, (int)(npc.position.Y / 16f) + 2) && npc.ai[1] <= 0f)
                {
                    npc.velocity.Y = -6f;
                    npc.ai[1] += 40f;
                }
            }
            //Main.NewText((int)((npc.position.X / 16) + npc.ai[2] * npc.direction) + "; " + (int)((npc.position.Y / 16f) + 2) + "; " + (int)(Main.MouseWorld.X / 16f) + "; " + (Main.MouseWorld.Y / 16f));     Testing stuff to see where detection is and where it needs to be
            if (npc.justHit)
            {
                npc.ai[0] = 0f;
            }
            if (npc.ai[0] >= 90f)
            {
                npc.ai[0] = 0f;
                Vector2 shootVel = target.Center - npc.Center;
                if (shootVel == Vector2.Zero)
                {
                    shootVel = new Vector2(0f, 1f);
                }
                shootVel.Normalize();
                shootVel *= 10f;
                int proj = Projectile.NewProjectile(npc.Center.X + (4f * npc.direction), npc.Center.Y - 5f, shootVel.X, shootVel.Y, mod.ProjectileType("MinerLightning"), npc.damage, 2f);
                Main.projectile[proj].netUpdate = true;
                npc.netUpdate = true;
            }
            if (target.position.X > npc.position.X)
            {
                npc.direction = 1;
                npc.ai[2] = 3f;
            }
            if (target.position.X < npc.position.X)
            {
                npc.direction = -1;
                npc.ai[2] = 1f;
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.SilverCoin, Main.rand.Next(1, 8));
            Item.NewItem(npc.getRect(), ItemID.CopperCoin, Main.rand.Next(1, 101));
            if (Main.rand.Next(0, 101) <= 45)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("ViralMeteorite"), Main.rand.Next(1, 6));
            }
        }

        private int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 48;
            npc.spriteDirection = npc.direction;
            if (npc.velocity != Vector2.Zero)
            {
                npc.frameCounter++;
                if (npc.frameCounter >= 10)
                {
                    frame += 1;
                    npc.frameCounter = 0;
                }
                if (frame >= 3)
                {
                    frame = 0;
                }
            }
            if (npc.ai[0] != 0f)
            {
                frame = 3;
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<MyPlayer>().ZoneViralMeteorite)
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