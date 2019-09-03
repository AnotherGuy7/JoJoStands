using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ProjectileBuff
{
    public class Spin : ModBuff
    {
        int directionCounter = 0;
        public override void SetDefaults()
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
            player.lifeRegen -= 60;
            player.moveSpeed /= 2;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            directionCounter++;
            if (directionCounter >= 5)
            {
                npc.direction *= -1;
                directionCounter = 0;
            }
            if (!npc.HasBuff(mod.BuffType("Spin")))
            {
                directionCounter = 0;
            }
            npc.AddBuff(BuffID.Confused, 95);
            npc.lifeRegen = (npc.lifeMax / 8) * -1;
            npc.velocity /= 2;
        }
    }
}