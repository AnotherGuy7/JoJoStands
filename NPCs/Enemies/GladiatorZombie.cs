using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class GladiatorZombie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 10;
        }

        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 48;
            npc.defense = 14;
            npc.lifeMax = 240;
            npc.damage = 34; 
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            npc.chaseable = true;
            npc.noGravity = false;
            npc.aiStyle = 0;
        }

        //npc.ai[0] = state (1 = Walking; 2 = Attacking)
        //npc.ai[1] = jump cooldown
        //npc.ai[2] = whether or not it's burning to death

        private const float MoveSpeed = 0.24f;

        public override void AI()
        {
            npc.AddBuff(mod.BuffType("Vampire"), 2);
            if (npc.HasBuff(mod.BuffType("Sunburn")))
            {
                npc.defense = 0;
                npc.damage = 0;
                npc.ai[2] = 1f;
            }

            Player target = Main.player[npc.target];
            if (target.dead || npc.target == -1)
            {
                npc.TargetClosest();
            }

            if (npc.ai[2] == 1f)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Smoke, Main.rand.NextFloat(-0.6f, 0.6f + 1f), Main.rand.NextFloat(-0.6f, 1f));
                return;
            }

            if (npc.ai[1] > 0)
            {
                npc.ai[1] -= 1;
            }
            if (npc.velocity.Y < 3f)
            {
                npc.velocity.Y += 0.05f;
            }

            if (Main.rand.Next(0, 251) <= 2)
            {
                Main.PlaySound(14, (int)npc.position.X, (int)npc.position.Y, 1, 1f, -1.6f);
            }

            float targetDistance = 0f;
            if (npc.target != -1)
            {
                 targetDistance = Vector2.Distance(npc.Center, Main.player[npc.target].Center);
            }

            if (targetDistance > 28f)
            {
                float direction = npc.position.X - target.position.X;
                if (direction < 0)
                {
                    npc.direction = 1;
                    npc.velocity.X = MoveSpeed;
                }
                else
                {
                    npc.direction = -1;
                    npc.velocity.X = -MoveSpeed;
                }
                if (WorldGen.SolidOrSlopedTile((int)(npc.position.X / 16) + (int)Math.Ceiling(npc.width / 16f) + 1, (int)(npc.position.Y / 16f) + (int)Math.Ceiling(npc.height / 16f) - 1) && npc.ai[1] <= 0f)
                {
                    npc.velocity.Y = -6f;
                    npc.frameCounter = -40;     //This is to delay animations
                    npc.ai[1] = 60f;
                }
            }
            else
            {
                npc.velocity.X = 0f;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            npc.ai[0] = 1f;
            if (npc.life < npc.lifeMax)
            {
                int lifeStealAmount = damage / 4;
                npc.life += lifeStealAmount;
            }
            if (npc.life > npc.lifeMax)
            {
                npc.life = npc.lifeMax;
            }

            if (Main.rand.Next(0, 101) <= 14)
            {
                target.AddBuff(BuffID.Poisoned, 300);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (npc.life < npc.lifeMax)
            {
                int lifeStealAmount = damage / 4;
                npc.life += lifeStealAmount;
            }
            if (npc.life > npc.lifeMax)
            {
                npc.life = npc.lifeMax;
            }
            target.AddBuff(BuffID.Poisoned, 300);
        }

        public override void NPCLoot()
        {
            int chance = Main.rand.Next(0, 101);
            int dropChance = 6;
            if (chance <= dropChance)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("RustedGladiatorHelmet"));
            }
            else if (chance <= dropChance * 2)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("RustedGladiatorChestplate"));
            }
            else if (chance <= dropChance * 3)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("RustedGladiatorBoots"));
            }

            Item.NewItem(npc.getRect(), ItemID.SilverCoin, Main.rand.Next(0, 3 + 1));
            Item.NewItem(npc.getRect(), ItemID.CopperCoin, Main.rand.Next(0, 99 + 1));
        }

        private int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 56;
            npc.frameCounter++;
            npc.spriteDirection = -npc.direction;
            if (npc.ai[0] == 0f)
            {
                if (npc.frameCounter >= 10)
                {
                    frame++;
                    npc.frameCounter = 0;
                    if (frame >= 7)
                    {
                        frame = 0;
                    }
                }
            }
            else if (npc.ai[0] == 1f)
            {
                if (frame <= 6)     //The end of idle frames
                {
                    frame = 7;
                }
                if (npc.frameCounter >= 9)
                {
                    frame++;
                    npc.frameCounter = 0;

                    if (frame >= Main.npcFrameCount[npc.type])
                    {
                        frame = 0;
                        npc.ai[0] = 0f;
                    }
                }
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float chance = 0f;
            if (JoJoStandsWorld.VampiricNight)
            {
                chance =SpawnCondition.OverworldNightMonster.Chance * 0.92f;
            }
            return chance;
        }
    }
}