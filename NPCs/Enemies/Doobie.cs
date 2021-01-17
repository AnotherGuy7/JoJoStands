using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class Doobie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 48;
            npc.defense = 24;
            npc.lifeMax = 400;
            npc.damage = 75; 
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            npc.chaseable = true;
            npc.noGravity = false;
            npc.daybreak = true;
            npc.aiStyle = 3;
            aiType = 73;
        }

        private int frame = 0;
        private int runCounter = 0;
        private int snakeLimit = 0;
        private int snakeCooldown = 0;
        private int typeofsnake = 0;
        private int lastdirection = 0;
        private int expertboost = 1;
        private bool waiting = true;

        public override void AI()
        {
            if (Main.expertMode)
            {
                expertboost = 2;
            }

            npc.AddBuff(mod.BuffType("Vampire"), 2);
            if (npc.HasBuff(mod.BuffType("Sunburn")) && waiting)
            {
                waiting = false;
                runCounter += 150;
            }

            if (npc.life > npc.lifeMax)
            {
                npc.life = npc.lifeMax;
            }

            Player target = Main.player[npc.target];
            if (target.dead || npc.target == -1)
            {
                npc.TargetClosest();
            }

            if (waiting)
            {
                npc.aiStyle = 0;
                npc.defense = 999999999;
                npc.velocity.X = 0;
                npc.knockBackResist = 0f;
                if (npc.velocity.Y != 0)
                {
                    waiting = false;
                    runCounter += 150;
                }
                if (npc.Distance(target.Center) <= 150f)
                {
                    waiting = false;
                    runCounter += 150;
                }
            }

            if (!waiting)
            {
                aiType = 73;
                npc.aiStyle = 3;
                if (npc.velocity.X > 0)
                {
                    npc.spriteDirection = 1;
                }
                if (npc.velocity.X < 0)
                {
                    npc.spriteDirection = -1;
                }
                if (runCounter > 0)
                {
                    npc.damage = 100 * expertboost;
                    npc.immortal = true;
                    runCounter -= 1;
                    npc.knockBackResist = 0f;
                    if (lastdirection == 0)
                    {
                        if (npc.position.X - 25 >= target.position.X)
                        {
                            npc.velocity.X = -4f;
                        }
                        if (npc.position.X + 25 < target.position.X)
                        {
                            npc.velocity.X = 4f;
                        }
                    }
                    if (lastdirection != 0)
                    {
                       npc.velocity.X = lastdirection * 4f;
                    }
                }
                if (runCounter == 0)
                {
                    npc.immortal = false;
                    if (snakeCooldown > 0)
                    {
                        snakeCooldown -= 1;
                    }
                    npc.knockBackResist = 0.5f;
                    if (npc.HasBuff(mod.BuffType("Sunburn")))
                    {
                        npc.defense = 0;
                        npc.damage = 30 * expertboost;
                    }
                    else
                    {
                        npc.defense = 24;
                        npc.damage = 50 * expertboost;
                    }
                }
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            if (waiting)
            {
                waiting = false;
                runCounter += 150;
            }
            if (runCounter == 0 && !waiting && snakeCooldown == 0 && snakeLimit < 7)
            {
                typeofsnake = Main.rand.Next(1, 3);
                if (typeofsnake != 0)
                {
                    if (typeofsnake == 1)
                    {
                        NPC.NewNPC((int)npc.Top.X, (int)npc.Top.Y, (int)mod.NPCType("DoobiesSnake"));
                        typeofsnake = 0;
                        snakeCooldown += 150;
                        snakeLimit += 1;
                    }
                    if (typeofsnake == 2)
                    {
                        NPC.NewNPC((int)npc.Top.X, (int)npc.Top.Y, (int)mod.NPCType("DoobiesChimeraSnake"));
                        typeofsnake = 0;
                        snakeCooldown += 150;
                        snakeLimit += 1;
                    }
                }
            }
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (waiting)
            {
                waiting = false;
                runCounter += 200;
            }
            if (runCounter == 0 && !waiting && snakeCooldown == 0 && snakeLimit < 7)
            {
                typeofsnake = Main.rand.Next(1, 3);
                if (typeofsnake != 0)
                {
                    snakeCooldown += 150;
                    snakeLimit += 1;
                    if (typeofsnake == 1)
                    {
                        NPC.NewNPC((int)npc.Top.X, (int)npc.Top.Y, (int)mod.NPCType("DoobiesSnake"));
                        typeofsnake = 0;
                    }
                    if (typeofsnake == 2)
                    {
                        NPC.NewNPC((int)npc.Top.X, (int)npc.Top.Y, (int)mod.NPCType("DoobiesChimeraSnake"));
                        typeofsnake = 0;
                    }
                }
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (runCounter > 0)
            {
                target.AddBuff(BuffID.Poisoned, 600);
            }
            if (runCounter == 0)
            {
                target.AddBuff(BuffID.Poisoned, 300);
            }
            if (npc.life < npc.lifeMax)
            {
                int lifeStealAmount = damage / 2;
                npc.life += lifeStealAmount;
            }
            if (lastdirection == 0)
            {
                lastdirection = npc.direction;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (runCounter > 0)
            {
                target.AddBuff(BuffID.Poisoned, 1200);
            }
            if (runCounter == 0)
            {
                target.AddBuff(BuffID.Poisoned, 600);
            }
            if (npc.life < npc.lifeMax)
            {
                int lifeStealAmount = damage / 2;
                npc.life += lifeStealAmount;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            frameHeight = 48;
            if (npc.velocity != Vector2.Zero)
            {
                npc.frameCounter += Math.Abs(npc.velocity.X);
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
            npc.frame.Y = frame * frameHeight;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (runCounter > 0)
            {
                SpriteEffects effects = (npc.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Texture2D texture2d = mod.GetTexture("NPCs/Enemies/Doobie");
                Vector2 vector2 = new Vector2((Main.npcTexture[npc.type].Width / 2), (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
                Main.spriteBatch.Draw(texture2d, new Vector2(npc.position.X - Main.screenPosition.X + (npc.width / 2) - Main.npcTexture[npc.type].Width * npc.scale / 2f + vector2.X * npc.scale, npc.position.Y - Main.screenPosition.Y + npc.height - Main.npcTexture[npc.type].Height * npc.scale / Main.npcFrameCount[npc.type] + 4f + vector2.Y * npc.scale), new Rectangle?(npc.frame), Color.White, npc.rotation, vector2, npc.scale, effects, 0f);
                for (int i = 1; i < npc.oldPos.Length; i++)
                {
                    Color color = Lighting.GetColor((int)(npc.position.X + npc.width * 0.5) / 16, (int)((npc.position.Y + npc.height * 0.5) / 16.0));
                    Color color2 = color;
                    color2 = Color.Lerp(color2, Color.Transparent, 0.5f);
                    color2 = npc.GetAlpha(color2);
                    color2 *= (npc.oldPos.Length - i) / 15f;
                    Main.spriteBatch.Draw(texture2d, new Vector2(npc.position.X - Main.screenPosition.X + (npc.width / 2) - Main.npcTexture[npc.type].Width * npc.scale / 2f + vector2.X * npc.scale, npc.position.Y - Main.screenPosition.Y + npc.height - Main.npcTexture[npc.type].Height * npc.scale / Main.npcFrameCount[npc.type] + 4f + vector2.Y * npc.scale) - npc.velocity * i * 0.5f, new Rectangle?(npc.frame), color2, npc.rotation, vector2, npc.scale, effects, 0f);
                }
            }
            return true;
        }
    }
}