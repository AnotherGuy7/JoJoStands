using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.World
{
    public class ViralMeteoriteBiome : ModBiome
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/VMMusic");
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override bool IsBiomeActive(Player player)
        {
            return JoJoStandsWorld.viralMeteoriteTiles > 20;
        }
    }
}
