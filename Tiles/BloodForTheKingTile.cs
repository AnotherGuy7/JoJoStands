using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ObjectData;

namespace JoJoStands.Tiles
{
    public class BloodForTheKingTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Width = 6;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                18
            };
            TileObjectData.addTile(Type);
            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Blood for the King>();
            AddMapEntry(new Color(120, 85, 60), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 4, 4, ModContent.ItemType<BloodForTheKing>());
        }
    }
}