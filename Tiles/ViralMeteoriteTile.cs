using JoJoStands.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace JoJoStands.Tiles
{
    public class ViralMeteoriteTile : ModTile
    {
        public override void SetStaticDefaults()      //some of this is from ExampleMod/Tiles/ExmapleOre
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;        //The tile will be affected by spelunker highlighting
            Main.tileShine2[Type] = true;       //Modifies the draw color slightly.
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileLighted[Type] = true;

            HitSound = SoundID.Tink;
            DustType = DustID.Silver;
            ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<ViralMeteorite>();
            MineResist = 3f;
            MinPick = 65;

            TileID.Sets.DisableSmartCursor[Type] = true;
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Viral Meteorite");
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
            if (tileTarget.TileType == TileID.Meteorite)
            {
                tileTarget.TileType = (ushort)ModContent.TileType<ViralMeteoriteTile>();
                WorldGen.SquareTileFrame(i + Xadd, j + Yadd, true);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, i + Xadd, j + Yadd, 1);
            }
        }
    }
}