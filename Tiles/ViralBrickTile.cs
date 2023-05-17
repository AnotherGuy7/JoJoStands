using JoJoStands.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Tiles
{
    public class ViralBrickTile : ModTile
    {
        public override void SetStaticDefaults()      //some of this is from ExampleMod/Tiles/ExmapleOre
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;

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