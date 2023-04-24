using JoJoStands.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace JoJoStands.Tiles
{
    public class BloodForTheKingTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Width = 6;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                18
            };
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Blood for the King");
            AddMapEntry(new Color(120, 85, 60), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 4, 4, ModContent.ItemType<BloodForTheKing>());
        }
    }
}