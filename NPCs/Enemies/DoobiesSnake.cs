using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class DoobiesSnake : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 48;
            NPC.defense = 12;
            NPC.lifeMax = 50;
            NPC.damage = 100;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.chaseable = true;
            NPC.noGravity = false;
            NPC.aiStyle = 3;
            AIType = 73;
        }

        private int frame = 0;
        public float JumpCooldown = 0;
        public override void AI()
        {
            Player target = Main.player[NPC.target];
            if (target.dead || NPC.target == -1)
            {
                NPC.TargetClosest();
            }
            if (NPC.velocity.X > 0)
            {
                NPC.spriteDirection = 1;
            }
            if (NPC.velocity.X < 0)
            {
                NPC.spriteDirection = -1;
            }
            if (NPC.velocity.Y < 0 && !NPC.collideX && JumpCooldown == 0f)
            {
                NPC.velocity.Y = 0;
            }
            if (NPC.collideX && JumpCooldown == 0f && NPC.velocity.Y == 0)
            {
                JumpCooldown = 45f;
                NPC.velocity.Y -= 8f;
            }
            if (NPC.velocity.Y < 3f)
            {
                NPC.velocity.Y += 0.3f;
            }
            if (JumpCooldown > 0f)
            {
                JumpCooldown--;
            }
            if (NPC.position.X - 25 >= target.position.X)
            {
                NPC.velocity.X = -0.75f;
            }
            if (NPC.position.X + 25 < target.position.X)
            {
                NPC.velocity.X = 0.75f;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Poisoned, 600);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit)
        {
            target.AddBuff(BuffID.Poisoned, 600);
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
    }
}