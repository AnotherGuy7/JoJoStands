using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class HazeVirusImmunity : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Haze Virus Immunity");
            // Description.SetDefault("You are immune to the Haze Virus and Concentrated Haze Virus.");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            // Continuously purge both haze debuffs while this buff is active
            player.ClearBuff(ModContent.BuffType<Debuffs.HazeVirus>());
            player.ClearBuff(ModContent.BuffType<Debuffs.ConcentratedHazeVirus>());
        }
    }
}
