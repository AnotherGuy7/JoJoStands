using JoJoStands.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace JoJoStands.Tiles
{
    public class QuietLifeTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16
            };
            TileObjectData.addTile(Type);
            TileID.Sets.DisableSmartCursor[Type] = true;
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Quiet Life");
            AddMapEntry(new Color(120, 85, 60), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 4, 4, ModContent.ItemType<QuietLife>());
        }
    }
}