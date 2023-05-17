using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace JoJoStands.NPCs.Enemies
{
    public class BaldZombie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 10;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 48;
            NPC.defense = 11;
            NPC.lifeMax = 180;
            NPC.damage = 26;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.7f;
            NPC.chaseable = true;
            NPC.noGravity = false;
            NPC.aiStyle = 0;
            NPC.value = 3 * 100;
        }

        //NPC.ai[0] = state (1 = Walking; 2 = Attacking)
        //NPC.ai[1] = jump cooldown
        //NPC.ai[2] = whether or not it's burning to death

        private const float MoveSpeed = 0.51f;

        public override void AI()
        {
            NPC.AddBuff(ModContent.BuffType<Vampire>(), 2);
            if (NPC.HasBuff(ModContent.BuffType<Sunburn>()))
            {
                NPC.defense = 0;
                NPC.damage = 0;
                NPC.ai[2] = 1f;
            }

            Player target = Main.player[NPC.target];
            if (target.dead || NPC.target == -1)
            {
                NPC.TargetClosest();
            }

            if (NPC.ai[2] == 1f)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, Main.rand.NextFloat(-0.6f, 0.6f + 1f), Main.rand.NextFloat(-0.6f, 1f));
                return;
            }

            if (NPC.ai[1] > 0)
            {
                NPC.ai[1] -= 1;
            }
            if (NPC.velocity.Y < 3f)
            {
                NPC.velocity.Y += 0.05f;
            }

            if (Main.rand.Next(0, 251) <= 2)
            {
                SoundStyle zombieMoan = SoundID.Zombie1;
                zombieMoan.Pitch = -1.6f;
                SoundEngine.PlaySound(zombieMoan, NPC.Center);
            }

            float targetDistance = 0f;
            if (NPC.target != -1)
            {
                targetDistance = Vector2.Distance(NPC.Center, Main.player[NPC.target].Center);
            }

            if (targetDistance > 28f)
            {
                float direction = NPC.position.X - target.position.X;
                if (direction < 0)
                {
                    NPC.direction = 1;
                    NPC.velocity.X = MoveSpeed;
                }
                else
                {
                    NPC.direction = -1;
                    NPC.velocity.X = -MoveSpeed;
                }
                if (WorldGen.SolidOrSlopedTile((int)(NPC.position.X / 16) + (int)Math.Ceiling(NPC.width / 16f) + 1, (int)(NPC.position.Y / 16f) + (int)Math.Ceiling(NPC.height / 16f) - 1) && NPC.ai[1] <= 0f)
                {
                    NPC.velocity.Y = -6f;
                    NPC.frameCounter = -40;     //This is to delay animations
                    NPC.ai[1] = 60f;
                }
            }
            else
            {
                NPC.velocity.X = 0f;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            NPC.ai[0] = 1f;
            if (NPC.life < NPC.lifeMax)
            {
                int lifeStealAmount = hurtInfo.Damage / 4;
                NPC.life += lifeStealAmount;
            }
            if (NPC.life > NPC.lifeMax)
                NPC.life = NPC.lifeMax;

            if (Main.rand.Next(0, 101) <= 14)
            {
                target.AddBuff(BuffID.Poisoned, 300);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit)
        {
            if (NPC.life < NPC.lifeMax)
            {
                int lifeStealAmount = hit.Damage / 4;
                NPC.life += lifeStealAmount;
            }
            if (NPC.life > NPC.lifeMax)
            {
                NPC.life = NPC.lifeMax;
            }
            target.AddBuff(BuffID.Poisoned, 300);
        }

        private int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 56;
            NPC.frameCounter++;
            NPC.spriteDirection = -NPC.direction;
            if (NPC.ai[0] == 0f)
            {
                if (NPC.frameCounter >= 8)
                {
                    frame++;
                    NPC.frameCounter = 0;
                    if (frame >= 7)
                    {
                        frame = 0;
                    }
                }
            }
            else if (NPC.ai[0] == 1f)
            {
                if (frame <= 6)     //The end of idle frames
                {
                    frame = 7;
                }
                if (NPC.frameCounter >= 9)
                {
                    frame++;
                    NPC.frameCounter = 0;

                    if (frame >= Main.npcFrameCount[NPC.type])
                    {
                        frame = 0;
                        NPC.ai[0] = 0f;
                    }
                }
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float chance = 0f;
            if (JoJoStandsWorld.VampiricNight)
                chance = SpawnCondition.OverworldNightMonster.Chance;

            return chance;
        }
    }
}