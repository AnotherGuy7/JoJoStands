using JoJoStands;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class DevilsPalmBiome : ModBiome
{
    //public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/DevilsPalm");

    public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

    public override bool IsBiomeActive(Player player)
    {
        int tilePosX = (int)(player.Center.X / 16);
        int tilePosY = (int)(player.Center.Y / 16);
        int count = 0;
        int radius = 85;

        for (int x = tilePosX - radius; x < tilePosX + radius; x++)
        {
            for (int y = tilePosY - radius; y < tilePosY + radius; y++)
            {
                if (!WorldGen.InWorld(x, y)) continue;
                Tile tile = Main.tile[x, y];
                if (tile.HasTile && tile.TileType == TileID.HardenedSand) //use custom tile later
                    count++;
            }
        }

        return count > 300;
    }
}