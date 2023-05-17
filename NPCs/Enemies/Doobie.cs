using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class Doobie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 48;
            NPC.defense = 24;
            NPC.lifeMax = 400;
            NPC.damage = 75;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            NPC.chaseable = true;
            NPC.noGravity = false;
            NPC.daybreak = true;
            NPC.aiStyle = 3;
            AIType = 73;
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

            NPC.AddBuff(ModContent.BuffType<Vampire>(), 2);
            if (NPC.HasBuff(ModContent.BuffType<Sunburn>()) && waiting)
            {
                waiting = false;
                runCounter += 150;
            }

            if (NPC.life > NPC.lifeMax)
            {
                NPC.life = NPC.lifeMax;
            }

            Player target = Main.player[NPC.target];
            if (target.dead || NPC.target == -1)
            {
                NPC.TargetClosest();
            }

            if (waiting)
            {
                NPC.aiStyle = 0;
                NPC.defense = 999999999;
                NPC.velocity.X = 0;
                NPC.knockBackResist = 0f;
                if (NPC.velocity.Y != 0)
                {
                    waiting = false;
                    runCounter += 150;
                }
                if (NPC.Distance(target.Center) <= 150f)
                {
                    waiting = false;
                    runCounter += 150;
                }
            }

            if (!waiting)
            {
                AIType = 73;
                NPC.aiStyle = 3;
                if (NPC.velocity.X > 0)
                {
                    NPC.spriteDirection = 1;
                }
                if (NPC.velocity.X < 0)
                {
                    NPC.spriteDirection = -1;
                }
                if (runCounter > 0)
                {
                    NPC.damage = 100 * expertboost;
                    NPC.immortal = true;
                    runCounter -= 1;
                    NPC.knockBackResist = 0f;
                    if (lastdirection == 0)
                    {
                        if (NPC.position.X - 25 >= target.position.X)
                        {
                            NPC.velocity.X = -4f;
                        }
                        if (NPC.position.X + 25 < target.position.X)
                        {
                            NPC.velocity.X = 4f;
                        }
                    }
                    if (lastdirection != 0)
                    {
                        NPC.velocity.X = lastdirection * 4f;
                    }
                }
                if (runCounter == 0)
                {
                    NPC.immortal = false;
                    if (snakeCooldown > 0)
                    {
                        snakeCooldown -= 1;
                    }
                    NPC.knockBackResist = 0.5f;
                    if (NPC.HasBuff(ModContent.BuffType<Sunburn>()))
                    {
                        NPC.defense = 0;
                        NPC.damage = 30 * expertboost;
                    }
                    else
                    {
                        NPC.defense = 24;
                        NPC.damage = 50 * expertboost;
                    }
                }
            }
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
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
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Top.X, (int)NPC.Top.Y, (int)ModContent.NPCType<DoobiesSnake>());
                        typeofsnake = 0;
                        snakeCooldown += 150;
                        snakeLimit += 1;
                    }
                    if (typeofsnake == 2)
                    {
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Top.X, (int)NPC.Top.Y, (int)ModContent.NPCType<DoobiesChimeraSnake>());
                        typeofsnake = 0;
                        snakeCooldown += 150;
                        snakeLimit += 1;
                    }
                }
            }
        }
        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
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
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Top.X, (int)NPC.Top.Y, (int)ModContent.NPCType<DoobiesSnake>());
                        typeofsnake = 0;
                    }
                    if (typeofsnake == 2)
                    {
                        NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Top.X, (int)NPC.Top.Y, (int)ModContent.NPCType<DoobiesChimeraSnake>());
                        typeofsnake = 0;
                    }
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (runCounter > 0)
            {
                target.AddBuff(BuffID.Poisoned, 600);
            }
            if (runCounter == 0)
            {
                target.AddBuff(BuffID.Poisoned, 300);
            }
            if (NPC.life < NPC.lifeMax)
            {
                int lifeStealAmount = hurtInfo.Damage / 2;
                NPC.life += lifeStealAmount;
            }
            if (lastdirection == 0)
            {
                lastdirection = NPC.direction;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit)
        {
            if (runCounter > 0)
            {
                target.AddBuff(BuffID.Poisoned, 1200);
            }
            if (runCounter == 0)
            {
                target.AddBuff(BuffID.Poisoned, 600);
            }
            if (NPC.life < NPC.lifeMax)
            {
                int lifeStealAmount = hit.Damage / 2;
                NPC.life += lifeStealAmount;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            frameHeight = 48;
            if (NPC.velocity != Vector2.Zero)
            {
                NPC.frameCounter += Math.Abs(NPC.velocity.X);
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
            NPC.frame.Y = frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (runCounter > 0)
            {
                SpriteEffects effects = (NPC.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Texture2D texture2d = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/NPCs/Enemies/Doobie").Value;
                Vector2 vector2 = new Vector2((TextureAssets.Npc[NPC.type].Value.Width / 2), (TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
                spriteBatch.Draw(texture2d, new Vector2(NPC.position.X - Main.screenPosition.X + (NPC.width / 2) - TextureAssets.Npc[NPC.type].Value.Width * NPC.scale / 2f + vector2.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - TextureAssets.Npc[NPC.type].Value.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + vector2.Y * NPC.scale), new Rectangle?(NPC.frame), Color.White, NPC.rotation, vector2, NPC.scale, effects, 0f);
                for (int i = 1; i < NPC.oldPos.Length; i++)
                {
                    Color color = Lighting.GetColor((int)(NPC.position.X + NPC.width * 0.5) / 16, (int)((NPC.position.Y + NPC.height * 0.5) / 16.0));
                    Color color2 = color;
                    color2 = Color.Lerp(color2, Color.Transparent, 0.5f);
                    color2 = NPC.GetAlpha(color2);
                    color2 *= (NPC.oldPos.Length - i) / 15f;
                    spriteBatch.Draw(texture2d, new Vector2(NPC.position.X - Main.screenPosition.X + (NPC.width / 2) - TextureAssets.Npc[NPC.type].Value.Width * NPC.scale / 2f + vector2.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - TextureAssets.Npc[NPC.type].Value.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + vector2.Y * NPC.scale) - NPC.velocity * i * 0.5f, new Rectangle?(NPC.frame), color2, NPC.rotation, vector2, NPC.scale, effects, 0f);
                }
            }
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float chance = 0f;
            if (JoJoStandsWorld.VampiricNight)
                chance = 0.01f;

            return chance;
        }
    }
}