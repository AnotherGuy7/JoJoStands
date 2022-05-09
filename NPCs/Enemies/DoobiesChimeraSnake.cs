using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.NPCs.Enemies
{

    public class DoobiesChimeraSnake : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/Minions/ChimeraSnake"; }
        }

        public override void SetDefaults()
        {
            npc.width = 60;
            npc.height = 62;
            npc.defense = 6;
            npc.lifeMax = 50;
            npc.damage = 100; 
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 1f;
            npc.chaseable = true;
            npc.aiStyle = 14;
        }

        private int frame = 0;

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
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 300);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 300);
        }
        public override void FindFrame(int frameHeight)
        {
            frameHeight = 62;
            npc.frameCounter += 1;
            if (npc.frameCounter >= 10)
            {
                frame += 1;
                npc.frameCounter = 0;
            }
            if (frame >= 3)
            {
                frame = 0;
            }
            npc.frame.Y = frame * frameHeight;
        }
    }
}