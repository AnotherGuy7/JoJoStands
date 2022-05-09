using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.NPCs.Enemies
{
    public class DoobiesSnake : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 48;
            npc.defense = 12;
            npc.lifeMax = 50;
            npc.damage = 100; 
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.chaseable = true;
            npc.noGravity = false;
            npc.aiStyle = 3;
            aiType = 73;
        }

        private int frame = 0;
        public float JumpCooldown = 0;
        public override void AI()
        {
            Player target = Main.player[npc.target];
            if (target.dead || npc.target == -1)
            {
                npc.TargetClosest();
            }
            if (npc.velocity.X > 0)
            {
                npc.spriteDirection = 1;
            }
            if (npc.velocity.X < 0)
            {
                npc.spriteDirection = -1;
            }
            if (npc.velocity.Y < 0 && !npc.collideX && JumpCooldown == 0f)
            {
                npc.velocity.Y = 0;
            }
            if (npc.collideX && JumpCooldown == 0f && npc.velocity.Y == 0) 
            {
                JumpCooldown = 45f;
                npc.velocity.Y -= 8f;
            }
            if (npc.velocity.Y < 3f)
            {
                npc.velocity.Y += 0.3f;
            }
            if (JumpCooldown > 0f)
            {
                JumpCooldown--;
            }
            if (npc.position.X - 25 >= target.position.X)
            {
                npc.velocity.X = -0.75f;
            }
            if (npc.position.X + 25 < target.position.X)
            {
                npc.velocity.X = 0.75f;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 600);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 600);
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
    }
}