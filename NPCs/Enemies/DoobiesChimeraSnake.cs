using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{

    public class DoobiesChimeraSnake : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/Minions/ChimeraSnake"; }
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 62;
            NPC.defense = 6;
            NPC.lifeMax = 50;
            NPC.damage = 100;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 1f;
            NPC.chaseable = true;
            NPC.aiStyle = 14;
        }

        private int frame = 0;

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
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Poisoned, 300);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit)
        {
            target.AddBuff(BuffID.Poisoned, 300);
        }
        public override void FindFrame(int frameHeight)
        {
            frameHeight = 62;
            NPC.frameCounter += 1;
            if (NPC.frameCounter >= 10)
            {
                frame += 1;
                NPC.frameCounter = 0;
            }
            if (frame >= 3)
            {
                frame = 0;
            }
            NPC.frame.Y = frame * frameHeight;
        }
    }
}