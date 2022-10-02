using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.NPCs.Enemies
{
    public class HighwayStar : ModNPC
    {
        private int frame = 0;
        private float velocity = 0f;
        private AnimationState animationState;
        private AnimationState oldAnimationState;
        private NPC NPCOwner;

        public enum AnimationState
        {
            Walking,
            TransformingInto,
            TransformingOutOf,
            Chasing
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 9;
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 32;
            NPC.defense = 5;
            NPC.lifeMax = 260;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 3f;
            NPC.chaseable = true;
            NPC.damage = 37;
            NPC.aiStyle = 0;
            NPC.noGravity = true;
        }

        public override bool CheckActive()
        {
            NPCOwner = Main.npc[(int)NPC.ai[2]];
            NPCOwner.ai[2] = 1f;
            return base.CheckActive();
        }

        public override bool CheckDead()
        {
            NPCOwner.ai[2] = 0f;
            return base.CheckDead();
        }

        public override void AI()       //aiStyle of Wyverns(87), I just needed the base AI of it and changed it to what I actually wanted it to do
        {
            Player target = Main.player[NPC.target];
            NPCOwner = Main.npc[(int)NPC.ai[2]];
            if (NPCOwner.ai[3] == 0f)      //if Yuya is dead
            {
                NPC.active = false;
                NPC.life = -1;
            }
            if (!NPC.active)        //if Highway Star is dead
            {
                NPC.ai[2] = 0f;
                return;
            }

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)        //if no target or that target is dead
                animationState = AnimationState.Walking;

            if (Math.Abs(target.velocity.X) <= 2f)     //if the target is standing still or moving slowly, to walk to the target
            {
                if (NPC.ai[0] <= 31f || NPC.ai[0] >= 1f)        //from 31 to 0, transform out
                {
                    NPC.ai[0]--;
                }
            }
            if (Math.Abs(target.velocity.X) > 2f)        //if the target's velocity is greater than 2 in either direction
            {
                animationState = AnimationState.Chasing;
                velocity = 6.1f;
                if (NPC.ai[0] <= 30f)        //from 0 to 31, transform in
                    NPC.ai[0]++;
            }
            if (NPC.ai[0] == 0f)        //the walking one
            {
                velocity = 2f;
                animationState = AnimationState.Walking;
            }
            if (NPC.ai[0] >= 1f && NPC.ai[0] <= 30f)        //the transformation one
            {
                animationState = AnimationState.TransformingInto;
            }
            NPC.TargetClosest(true);        //walking velocity is 2-3f, diagonal velocity (flying with wings) is 8-9f, velocity with a UFO is 7-8f, most boots have 5-6f, while Lightning and Frostpark boots have 6.5-6.7f (all for players)
            if (NPC.ai[0] == 31f)        //chasing
            {
                velocity = 5f;
            }
            float num2 = 5f;
            if (NPC.direction == -1 && NPC.velocity.X > -num2)
            {
                NPC.velocity.X = -velocity;
                if (NPC.velocity.X > num2)
                {
                    NPC.velocity.X = NPC.velocity.X - 0.1f;
                }
                else if (NPC.velocity.X > 0f)
                {
                    NPC.velocity.X = NPC.velocity.X + 0.05f;
                }
                if (NPC.velocity.X < -num2)
                {
                    NPC.velocity.X = -num2;
                }
            }
            else if (NPC.direction == 1 && NPC.velocity.X < num2)
            {
                NPC.velocity.X = velocity;
                if (NPC.velocity.X < -num2)
                {
                    NPC.velocity.X = NPC.velocity.X + 0.1f;
                }
                else if (NPC.velocity.X < 0f)
                {
                    NPC.velocity.X = NPC.velocity.X - 0.05f;
                }
                if (NPC.velocity.X > num2)
                {
                    NPC.velocity.X = num2;
                }
            }
            if (NPC.directionY == -1 && target.position.Y <= NPC.position.Y)        //going up
            {
                NPC.velocity.Y = -3.05f;
            }
            else if (NPC.directionY == 1 && target.position.Y >= NPC.position.Y)
            {
                NPC.velocity.Y = 3.05f;
            }
            if (NPC.ai[0] <= 0f)
            {
                NPC.ai[0] = 0f;
            }
            if (NPC.ai[0] >= 32f)
            {
                NPC.ai[0] = 31f;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (oldAnimationState != animationState)
            {
                oldAnimationState = animationState;
                NPC.frameCounter = 0;
                frame = 0;
            }

            frameHeight = 50;       //NPC.frame.Y to x * frameHeight
            NPC.frameCounter++;
            if (animationState == AnimationState.Walking)
            {
                if (NPC.frameCounter >= 20)
                {
                    frame += 1;
                    NPC.frameCounter = 0;
                    if (frame >= 4)
                        frame = 1;
                }
                if (frame == 0)
                    frame = 1;
            }
            if (animationState == AnimationState.TransformingInto)
            {
                if (NPC.frameCounter >= 15)
                {
                    frame += 1;
                    NPC.frameCounter = 0;
                    if (frame >= 7)
                        frame = 5;
                }
                if (frame < 5)
                    frame = 5;
            }
            if (animationState == AnimationState.TransformingOutOf)
            {
                if (NPC.frameCounter >= 15)
                {
                    frame -= 1;
                    NPC.frameCounter = 0;
                    if (frame >= 7)
                        frame = 7;
                }
                if (frame < 5)
                    frame = 5;

            }
            if (animationState == AnimationState.Chasing)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 60)
                {
                    frame += 1;
                    NPC.frameCounter = 0;
                    if (frame >= 9)
                        frame = 7;
                }
                if (frame < 7)
                    frame = 7;
            }
            NPC.frame.Y = frame * frameHeight;
        }
    }
}