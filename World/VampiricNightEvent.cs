using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.World
{
    public class VampiricNightEvent : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player) => JoJoStandsWorld.VampiricNight;

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/VNight");
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
    }
}
