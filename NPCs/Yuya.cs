using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.NPCs
{
    public class Yuya : ModNPC
    {
        public int standcounter = 0;
        public int frame = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 16; //this defines how many frames the npc sprite sheet has
        }

        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 32;
            npc.defense = 5;
            npc.lifeMax = 110;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 1f;
            npc.chaseable = true;
            npc.damage = 37;
            npc.aiStyle = 0;
        }

        //npc.ai[2] = is stand alive
        //npc.ai[3] = is user alive

        public override bool CheckActive()
        {
            npc.ai[3] = 1f;
            return base.CheckActive();
        }

        public override bool CheckDead()
        {
            npc.ai[3] = 0f;
            npc.ai[2] = 0f;
            return base.CheckDead();
        }

        public override void AI()       //a really trashy custom ai...
        {
            npc.spriteDirection = npc.direction;
            if (npc.active)
            {
                npc.ai[0]++;
            }
            if (npc.ai[0] == 1f && npc.ai[2] == 0f)
            {
                NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("HighwayStar"), 0, 0f, 0f, npc.whoAmI, 0f, npc.target);
            }
            if (npc.ai[2] == 0f)
            {
                standcounter++;
            }
            if (standcounter == 180)      //once the stand is dead, spawn another one but only one more time, which is why I never set standcounter to restart
            {
                npc.ai[0] = 0f;
            }
            if (npc.justHit)        //run the other way when hit
            {
                if (npc.direction == 1) 
                {
                    npc.ai[1] = 1f;
                }
                if (npc.direction == -1)
                {
                    npc.spriteDirection = 1;
                    npc.direction = 1;
                    npc.ai[1] = 2f;
                }
            }
            if (npc.ai[1] == 1f)
            {
                npc.spriteDirection = -1;
                npc.direction = -1;
                npc.velocity.X = -2f;
            }
            if (npc.ai[1] == 2f)
            {
                npc.spriteDirection = 1;
                npc.direction = 1;
                npc.velocity.X = 2f;
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.GoldCoin, Main.rand.Next(1, 5));
            Item.NewItem(npc.getRect(), ItemID.SilverCoin, Main.rand.Next(1, 100));
        }

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 44;
            npc.spriteDirection = npc.direction;
            npc.frameCounter++;
            if (npc.frameCounter >= 10)
            {
                npc.frameCounter = 0;
            }
            if (npc.life == npc.lifeMax)        //stand still frames
            {
                frame = 1;
            }
            if (npc.ai[1] == 1f || npc.ai[1] == 2f)        //run if he got hit frames
            {
                if (npc.frameCounter <= 10)
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
                npc.frame.Y = frame * frameHeight;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode)
            {
                return SpawnCondition.OverworldNightMonster.Chance * 0.07f;
            }
            return 0f;
        }
    }
}