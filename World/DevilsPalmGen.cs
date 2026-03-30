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
            // By inserting after "Micro Biomes", we ensure the Caves, 
            // Corruption/Crimson, and standard structures are already done generating.
            // This prevents them from carving through or corrupting the Devil's Palm.
            int insertIdx = tasks.FindIndex(p => p.Name.Equals("Micro Biomes", StringComparison.OrdinalIgnoreCase));

            // Fallback: If "Micro Biomes" isn't found for some reason, try to inject after "Corruption" 
            // (Note: The "Corruption" pass handles both Crimson and Corruption generation)
            if (insertIdx == -1)
            {
                insertIdx = tasks.FindIndex(p => p.Name.Equals("Corruption", StringComparison.OrdinalIgnoreCase));
            }

            // Absolute fallback: put it near the end
            if (insertIdx == -1)
            {
                insertIdx = tasks.Count - 1;
            }

            tasks.Insert(insertIdx + 1, new DevilsPalmPass());
        }
    }
}