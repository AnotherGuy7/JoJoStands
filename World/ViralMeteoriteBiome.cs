using Microsoft.Xna.Framework;
using System;
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

        public override void OnEnter(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                JoJoStandsShaders.ActivateShader(JoJoStandsShaders.ViralMeteoriteEffect);
                if (!JoJoStandsWorld.VisitedViralMeteorite)
                {
                    JoJoStandsWorld.VisitedViralMeteorite = true;
                    Main.NewText("Your mind and soul resonate with the area around you...", JoJoStandsWorld.WorldEventTextColor);
                }
            }
        }

        public override void OnLeave(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
                JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.ViralMeteoriteEffect);
        }

        public override void OnInBiome(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
                JoJoStandsShaders.ChangeShaderUseProgress(JoJoStandsShaders.ViralMeteoriteEffect, Math.Clamp(1f - ((Vector2.Distance(player.Center, JoJoStandsWorld.ViralMeteoriteCenter.ToVector2()) * 1.6f) / (Main.screenWidth * 2f)), 0f, 1f));
        }
    }
}
