using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class Spin : ModBuff
    {
        public static int directionCounter = 0;
        private int correctHP = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spin");
            Description.SetDefault("You're being infinitely spun... There's no hope in surviving.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.direction *= -1;
            player.lifeRegen = -60;
            player.moveSpeed /= 2;
            player.buffTime[buffIndex] = 2;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (correctHP == 0)
                correctHP = npc.life;
            if (npc.boss && correctHP - npc.life >= npc.lifeMax / 4 && correctHP != 0)
            {
                npc.DelBuff(buffIndex);
                correctHP = 0;
            }
            directionCounter++;
            if (directionCounter >= 5)
            {
                npc.direction *= -1;
                directionCounter = 0;
            }
            if (!npc.HasBuff(ModContent.BuffType<Spin>()))
                directionCounter = 0;

            npc.AddBuff(BuffID.Confused, 95);
            npc.lifeRegen = (npc.lifeMax / 8) * -1;
            npc.buffTime[buffIndex] = 2;
            if (Math.Abs(npc.velocity.X) > 1f)
                npc.velocity.X *= 0.5f;
        }
    }
}