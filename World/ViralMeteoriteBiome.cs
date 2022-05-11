using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.World
{
    public class ViralMeteoriteBiome : ModBiome
    {
        public override bool IsBiomeActive(Player player)
        {
            return JoJoStandsWorld.viralMeteoriteTiles > 20;
        }
    }
}
