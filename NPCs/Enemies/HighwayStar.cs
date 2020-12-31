using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class HighwayStar : ModNPC
    {
        public bool walkFrames = false;
        public bool chaseFrames = false;
        public bool transformFramesInto = false;
        public bool transformFramesOutof = false;
        public int frame = 0;
        public float velocity = 0f;
        public NPC npcOwner;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 9;
        }

        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 32;
            npc.defense = 5;
            npc.lifeMax = 260;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 3f;
            npc.chaseable = true;
            npc.damage = 37;
            npc.aiStyle = 0;
            npc.noGravity = true;
        }

        public override bool CheckActive()
        {
            npcOwner = Main.npc[(int)npc.ai[2]];
            npcOwner.ai[2] = 1f;
            return base.CheckActive();
        }

        public override bool CheckDead()
        {
            npcOwner.ai[2] = 0f;
            return base.CheckDead();
        }

        public override void AI()       //aiStyle of Wyverns(87), I just needed the base AI of it and changed it to what I actually wanted it to do
        {
            Player target = Main.player[npc.target];
            npcOwner = Main.npc[(int)npc.ai[2]];
            npc.noGravity = true;
            if (npcOwner.ai[3] == 0f)      //if Yuya is dead
            {
                npc.active = false;
                npc.life = -1;
            }
            if (!npc.active)        //if Highway Star is dead
            {
                npc.ai[2] = 0f;
            }
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)        //if no target or that target is dead
            {
                walkFrames = true;
                chaseFrames = false;
                transformFramesInto = false;
                transformFramesOutof = false;
            }
            if (target.velocity.X <= 2f && target.velocity.X >= -2f)     //if the target is standing still or moving slowly, to walk to the target
            {
                if (npc.ai[0] <= 31f || npc.ai[0] >= 1f)        //from 31 to 0, transform out
                {
                    npc.ai[0]--;
                }
            }
            if (target.velocity.X >= 2f || target.velocity.X <= -2f)        //if the target's velocity is greater than 2 in either direction
            {
                chaseFrames = true;
                walkFrames = false;
                transformFramesInto = false;
                transformFramesOutof = false;
                velocity = 6.1f;
                if (npc.ai[0] >= 0f || npc.ai[0] <= 30f)        //from 0 to 31, transform in
                {
                    npc.ai[0]++;
                }
            }
            if (npc.ai[0] == 0f)        //the walking one
            {
                velocity = 2f;
                walkFrames = true;
                chaseFrames = false;
                transformFramesInto = false;
                transformFramesOutof = false;
            }
            if (npc.ai[0] >= 1f && npc.ai[0] <= 30f)        //the transformation one
            {
                if (target.velocity.X >= 2f || target.velocity.X <= -2f)
                {
                    transformFramesInto = true;
                    transformFramesOutof = false;
                    walkFrames = false;
                    chaseFrames = false;
                }
                if (target.velocity.X == 0f)
                {
                    transformFramesInto = true;
                    transformFramesOutof = false;
                    walkFrames = false;
                    chaseFrames = false;
                }
            }
            npc.TargetClosest(true);        //walking velocity is 2-3f, diagonal velocity (flying with wings) is 8-9f, velocity with a UFO is 7-8f, most boots have 5-6f, while Lightning and Frostpark boots have 6.5-6.7f (all for players)
            if (npc.ai[0] == 31f)        //chasing
            {
                velocity = 5f;
            }
            float num2 = 5f;
            if (npc.direction == -1 && npc.velocity.X > -num2)
            {
                npc.velocity.X = -velocity;
                if (npc.velocity.X > num2)
                {
                    npc.velocity.X = npc.velocity.X - 0.1f;
                }
                else if (npc.velocity.X > 0f)
                {
                    npc.velocity.X = npc.velocity.X + 0.05f;
                }
                if (npc.velocity.X < -num2)
                {
                    npc.velocity.X = -num2;
                }
            }
            else if (npc.direction == 1 && npc.velocity.X < num2)
            {
                npc.velocity.X = velocity;
                if (npc.velocity.X < -num2)
                {
                    npc.velocity.X = npc.velocity.X + 0.1f;
                }
                else if (npc.velocity.X < 0f)
                {
                    npc.velocity.X = npc.velocity.X - 0.05f;
                }
                if (npc.velocity.X > num2)
                {
                    npc.velocity.X = num2;
                }
            }
            if (npc.directionY == -1 && target.position.Y <= npc.position.Y)        //going up
            {
                npc.velocity.Y = -3.05f;
            }
            else if (npc.directionY == 1 && target.position.Y >= npc.position.Y)
            {
                npc.velocity.Y = 3.05f;
            }
            if (npc.ai[0] <= 0f)
            {
                npc.ai[0] = 0f;
            }
            if (npc.ai[0] >= 32f)
            {
                npc.ai[0] = 31f;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 50;       //npc.frame.Y to x * frameHeight
            npc.frameCounter++;
            if (npc.frameCounter >= 61)
            {
                npc.frameCounter = 0;
            }
            if (walkFrames)
            {
                transformFramesInto = false;
                transformFramesOutof = false;
                chaseFrames = false;
                if (npc.frameCounter == 20)
                {
                    frame += 1;
                    npc.frameCounter = 0;
                }
                if (frame >= 4)
                {
                    frame = 1;
                }
                if (frame <= 0)
                {
                    frame = 1;
                }
            }
            if (transformFramesInto)
            {
                walkFrames = false;
                chaseFrames = false;
                transformFramesOutof = false;
                if (npc.frameCounter >= 15)
                {
                    frame += 1;
                    npc.frameCounter = 0;
                }
                if (frame <= 5)
                {
                    frame = 5;
                }
                if (frame >= 7)
                {
                    frame = 5;
                }
            }
            if (transformFramesOutof)
            {
                walkFrames = false;
                chaseFrames = false;
                transformFramesOutof = false;
                if (npc.frameCounter >= 15)
                {
                    frame -= 1;
                    npc.frameCounter = 0;
                }
                if (frame <= 5)
                {
                    frame = 5;
                }
                if (frame >= 7)
                {
                    frame = 7;
                }
            }
            if (chaseFrames)
            {
                walkFrames = false;
                transformFramesInto = false;
                transformFramesOutof = false;
                frame++;
                if (npc.frameCounter == 60)
                {
                    frame += 1;
                }
                if (frame <= 7)
                {
                    frame = 7;
                }
                if (frame >= 9)
                {
                    frame = 7;
                }
            }
            npc.frame.Y = frame * frameHeight;
        }
    }
}