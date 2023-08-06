using JoJoStands.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace JoJoStands.Tiles
{
    public class ViralToiletTile : ModTile
    {
        public override void SetStaticDefaults()      //some of this is from ExampleMod/Tiles/ExmapleOre
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(200, 200, 200));
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.248f;
            g = 0.222f;
            b = 0.126f;
            Lighting.AddLight(new Vector2(i, j), r / 2f, g / 2f, b / 2f);
        }
    }
}