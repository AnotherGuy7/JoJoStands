using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace JoJoStands.Tiles
{
    public class ViralMeteoriteTile : ModTile
    {
        public override void SetDefaults()      //some of this is from ExampleMod/Tiles/ExmapleOre
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileID.Sets.Ore[Type] = true;
            Main.tileValue[Type] = 309;
            Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.addTile(Type);

            soundType = SoundID.Tink;
            dustType = DustID.Silver;
            drop = mod.ItemType("ViralMeteorite");
            mineResist = 3f;
            minPick = 65;

            disableSmartCursor = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Viral Meteorite");
            AddMapEntry(Color.LightGray, name);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.248f;
            g = 0.222f;
            b = 0.126f;
            Lighting.AddLight(new Vector2(i, j), r, g, b);
        }

        public override void RandomUpdate(int i, int j)
        {
            int Xadd = 0;
            int Yadd = 0;
            int checkNumber = Main.rand.Next(0, 4);     //just detects one side of a tile whenever this updates
            if (checkNumber == 0)
            {
                Xadd = 1;
                Yadd = 0;
            }
            if (checkNumber == 1)
            {
                Xadd = 0;
                Yadd = 1;
            }
            if (checkNumber == 2)
            {
                Xadd = -1;
                Yadd = 0;
            }
            if (checkNumber == 3)
            {
                Xadd = 0;
                Yadd = -1;
            }
            Tile tileTarget = Main.tile[i + Xadd, j + Yadd];
            if (tileTarget.type == TileID.Meteorite)
            {
                tileTarget.type = (ushort)mod.TileType(Name);
                WorldGen.SquareTileFrame(i + Xadd, j + Yadd, true);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendTileSquare(-1, i + Xadd, j + Yadd, 1);
                }
            }
        }
    }
}