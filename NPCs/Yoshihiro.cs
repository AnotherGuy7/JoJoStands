using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.NPCs
{
    public class Yoshihiro : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 11; //this defines how many frames the npc sprite sheet has
        }

        public override void SetDefaults()
        {
            npc.width = 38; //the npc sprite width
            npc.height = 32;  //the npc sprite height
            //this is the npc ai style, 7 is Pasive Ai. 3 is the fighter AI
            npc.defense = 13;  //the npc defense
            npc.lifeMax = 180;  // the npc life
            npc.HitSound = SoundID.NPCHit1;  //the npc sound when is hit
            npc.DeathSound = SoundID.NPCDeath1;  //the npc sound when he dies
            npc.knockBackResist = 2f;  //the npc knockback resistance
            npc.chaseable = true;       //whether or not minions can chase this npc
            npc.damage = 37;       //the damage the npc does
            animationType = 122;
            npc.aiStyle = 0;        //no AI, to run void AI()
            npc.noGravity = true;
        }

        public override void AI()
        {
            bool flag19 = false;        ////recreating aiStyle 22 directly from source, type 122
            if (npc.justHit)
            {
                npc.ai[2] = 0f;
            }
            int num284 = (int)((npc.position.X + (float)(npc.width / 2)) / 16f) + npc.direction * 2;
            int num285 = (int)((npc.position.Y + (float)npc.height) / 16f);
            bool flag23 = true;
            bool flag24 = false;
            int num286 = 3;
            if (npc.type == 122 || true)
            {
                if (npc.justHit)
                {
                    npc.ai[3] = 0f;
                    npc.localAI[1] = 0f;
                }
                float num287 = 7f;
                Vector2 vector33 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float num288 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector33.X;
                float num289 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector33.Y;
                float num290 = (float)Math.Sqrt((double)(num288 * num288 + num289 * num289));
                num290 = num287 / num290;
                num288 *= num290;
                num289 *= num290;
                if (Main.netMode != 1 && npc.ai[3] == 32f && !Main.player[npc.target].npcTypeNoAggro[npc.type])
                {
                    int num291 = 25;
                    int num292 = mod.ProjectileType("Yosarrow");        //what Yoshihiro will shoot
                    Projectile.NewProjectile(vector33.X, vector33.Y, num288, num289, num292, num291, 0f, Main.myPlayer, 0f, 0f);
                }
                num286 = 8;
                if (npc.ai[3] > 0f)
                {
                    npc.ai[3] += 1f;
                    if (npc.ai[3] >= 64f)
                    {
                        npc.ai[3] = 0f;
                    }
                }
                if (Main.netMode != 1 && npc.ai[3] == 0f)
                {
                    npc.localAI[1] += 1f;
                    if (npc.localAI[1] > 120f && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && !Main.player[npc.target].npcTypeNoAggro[npc.type])
                    {
                        npc.localAI[1] = 0f;
                        npc.ai[3] = 1f;
                        npc.netUpdate = true;
                    }
                }
            }
            int num;
            for (int num309 = num285; num309 < num285 + num286; num309 = num + 1)
            {
                if (Main.tile[num284, num309] == null)
                {
                    Main.tile[num284, num309] = new Tile();
                }
                if ((Main.tile[num284, num309].nactive() && Main.tileSolid[(int)Main.tile[num284, num309].type]) || Main.tile[num284, num309].liquid > 0)
                {
                    if (num309 <= num285 + 1)
                    {
                        flag24 = true;
                    }
                    flag23 = false;
                    break;
                }
                num = num309;
            }
            if (Main.player[npc.target].npcTypeNoAggro[npc.type])
            {
                bool flag25 = false;
                for (int num310 = num285; num310 < num285 + num286 - 2; num310 = num + 1)
                {
                    if (Main.tile[num284, num310] == null)
                    {
                        Main.tile[num284, num310] = new Tile();
                    }
                    if ((Main.tile[num284, num310].nactive() && Main.tileSolid[(int)Main.tile[num284, num310].type]) || Main.tile[num284, num310].liquid > 0)
                    {
                        flag25 = true;
                        break;
                    }
                    num = num310;
                }
                npc.directionY = (!flag25).ToDirectionInt();
            }
            if (flag19)
            {
                flag24 = false;
                flag23 = true;
            }
            if (flag23)
            {
                npc.velocity.Y = npc.velocity.Y + 0.1f;
                if (npc.velocity.Y > 3f)
                {
                    npc.velocity.Y = 3f;
                }
            }
            else
            {
                if (npc.directionY < 0 && npc.velocity.Y > 0f)
                {
                    npc.velocity.Y = npc.velocity.Y - 0.1f;
                }
                if (npc.velocity.Y < -4f)
                {
                    npc.velocity.Y = -4f;
                }
            }
            if (npc.collideX)
            {
                npc.velocity.X = npc.oldVelocity.X * -0.4f;
                if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 1f)
                {
                    npc.velocity.X = 1f;
                }
                if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -1f)
                {
                    npc.velocity.X = -1f;
                }
            }
            if (npc.collideY)
            {
                npc.velocity.Y = npc.oldVelocity.Y * -0.25f;
                if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
                {
                    npc.velocity.Y = 1f;
                }
                if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
                {
                    npc.velocity.Y = -1f;
                }
            }
            float num312 = 2f;     //this was originally 2f
            if (npc.direction == -1 && npc.velocity.X > -num312)        //if it's facing the left
            {
                npc.velocity.X = -1.5f;
                if (npc.velocity.X > num312)
                {
                    npc.velocity.X = npc.velocity.X - 0.1f;
                }
                if (npc.velocity.X > -1.5f)
                {
                    npc.velocity.X = npc.velocity.X + 1f;
                }
                if (npc.velocity.X < -num312)
                {
                    npc.velocity.X = -num312;
                }
            }
            else if (npc.direction == 1 && npc.velocity.X < num312)     //if it's facing the right
            {
                npc.velocity.X = 1.5f;
                if (npc.velocity.X < -num312)
                {
                    npc.velocity.X = npc.velocity.X + 0.1f;
                }
                if (npc.velocity.X < 1.5f)
                {
                    npc.velocity.X = npc.velocity.X - 1f;
                }
                if (npc.velocity.X > num312)
                {
                    npc.velocity.X = num312;
                }
            }
            num312 = 1.5f;      //this was originally if (npc.type == 490) {num312 = 1f;} else {num312 = 1.5f}
            if (npc.directionY == -1 && npc.velocity.Y > -num312)
            {
                npc.velocity.Y = npc.velocity.Y - 0.04f;
                if (npc.velocity.Y > num312)
                {
                    npc.velocity.Y = npc.velocity.Y - 0.05f;
                }
                else if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y = npc.velocity.Y + 0.03f;
                }
                if (npc.velocity.Y < -num312)
                {
                    npc.velocity.Y = -num312;
                }
            }
            else if (npc.directionY == 1 && npc.velocity.Y < num312)
            {
                npc.velocity.Y = npc.velocity.Y + 0.04f;
                if (npc.velocity.Y < -num312)
                {
                    npc.velocity.Y = npc.velocity.Y + 0.05f;
                }
                else if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y = npc.velocity.Y - 0.03f;
                }
                if (npc.velocity.Y > num312)
                {
                    npc.velocity.Y = num312;
                }
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), mod.ItemType("StandArrow"), 1);
        }
    }
}