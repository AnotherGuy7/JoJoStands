using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using Terraria.Utilities;

namespace JoJoStands
{
    public class DevilsPalmGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int insertIdx = tasks.FindIndex(p => p.Name.Equals("Micro Biomes", StringComparison.OrdinalIgnoreCase));
            if (insertIdx == -1)
                insertIdx = tasks.FindIndex(p => p.Name.Equals("Corruption", StringComparison.OrdinalIgnoreCase));
            if (insertIdx == -1)
                insertIdx = tasks.Count - 1;
            tasks.Insert(insertIdx + 1, new DevilsPalmPass());
        }
    }
}