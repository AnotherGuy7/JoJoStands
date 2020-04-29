using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class Locked : ModBuff
    {
        public int Counter = 0;
        public int lifeRegenIncrement = 0;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Locked");
            Description.SetDefault("Your guilt is increasing and it hurts.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen = -4;
            player.moveSpeed *= 0.95f;
            Counter++;
            player.statDefense = (int)(player.statDefense * 0.95);
            if (Counter >= 60)    //increases lifeRegen damage every second
            {
                Counter = 0;
                lifeRegenIncrement += 2;
            }
            player.lifeRegen = -4 - lifeRegenIncrement;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen = -4;
            npc.velocity *= 0.95f;
            Counter++;
            npc.defense = (int)(npc.defense * 0.95);
            if (Counter >= 60)    //increases lifeRegen damage every second
            {
                Counter = 0;
                lifeRegenIncrement += 2;
                npc.StrikeNPC(lifeRegenIncrement, 0f, 1);
            }
        }
    }
}