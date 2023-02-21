using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.DataStructures
{
    public struct DestroyedTileData         
    {
        public ushort TileType { get; set; }
        public Vector2 TilePosition { get; set; }
        public SlopeType TileSlope { get; set; }
        public bool HalfTile { get; set; }
        public short TileY { get; set; }
        public short TileX { get; set; }
        public int TileFrame { get; set; }
        public byte TileColor { get; set; }
        public byte TileWallColor { get; set; }

        public DestroyedTileData(ushort tileType, Vector2 tilePosition, SlopeType tileSlope, bool tileHalf, short tileY, short tileX, int tileFrame, byte tileColor, byte tileWallColor)
        {
            TileType = tileType;
            TilePosition = tilePosition;
            TileSlope = tileSlope;
            HalfTile = tileHalf;
            TileY = tileY;
            TileX = tileX;
            TileFrame = tileFrame;
            TileColor = tileColor;
            TileWallColor = tileWallColor;
        }

        public static void Destroy(DestroyedTileData ExtraTile)
        {
            Vector2 vector2 = ExtraTile.TilePosition;
            WorldGen.KillTile((int)vector2.X, (int)vector2.Y, false, false, true);
            NetMessage.SendTileSquare(-1, (int)vector2.X, (int)vector2.Y, 1);
        }

        public static void Restore(DestroyedTileData tileData)
        {
            Vector2 vector2 = tileData.TilePosition;
            Tile tile = Main.tile[(int)vector2.X, (int)vector2.Y];
            if (!tile.HasTile)
            {
                if (tileData.TileType != TileID.Grass || tileData.TileType != TileID.CorruptGrass || tileData.TileType != TileID.CrimsonGrass || tileData.TileType != TileID.HallowedGrass || tileData.TileType != TileID.JungleGrass || tileData.TileType != TileID.MushroomGrass || tileData.TileType != TileID.GolfGrass || tileData.TileType != TileID.GolfGrassHallowed)
                    WorldGen.PlaceTile((int)vector2.X, (int)vector2.Y, tileData.TileType, false, true);
                if (tileData.TileType == TileID.Grass || tileData.TileType == TileID.CorruptGrass || tileData.TileType == TileID.CrimsonGrass || tileData.TileType == TileID.HallowedGrass || tileData.TileType == TileID.GolfGrass || tileData.TileType == TileID.GolfGrassHallowed)
                    Restore2(TileID.Dirt, TileID.Grass, vector2);
                if (tileData.TileType == TileID.CorruptGrass || tileData.TileType == TileID.CrimsonGrass || tileData.TileType == TileID.HallowedGrass || tileData.TileType == TileID.GolfGrass || tileData.TileType == TileID.GolfGrassHallowed)
                    Restore2(TileID.Dirt, TileID.CorruptGrass, vector2);
                if (tileData.TileType == TileID.CrimsonGrass || tileData.TileType == TileID.HallowedGrass || tileData.TileType == TileID.GolfGrass || tileData.TileType == TileID.GolfGrassHallowed)
                    Restore2(TileID.Dirt, TileID.CrimsonGrass, vector2);
                if (tileData.TileType == TileID.HallowedGrass || tileData.TileType == TileID.GolfGrass || tileData.TileType == TileID.GolfGrassHallowed)
                    Restore2(TileID.Dirt, TileID.HallowedGrass, vector2);
                if (tileData.TileType == TileID.GolfGrass || tileData.TileType == TileID.GolfGrassHallowed)
                    Restore2(TileID.Dirt, TileID.GolfGrass, vector2);
                if (tileData.TileType == TileID.GolfGrassHallowed)
                    Restore2(TileID.Dirt, TileID.GolfGrassHallowed, vector2);
                if (tileData.TileType == TileID.JungleGrass)
                    Restore2(TileID.Mud, TileID.JungleGrass, vector2);
                if (tileData.TileType == TileID.MushroomGrass)
                    Restore2(TileID.Mud, TileID.MushroomGrass, vector2);
                if (tile.TileType == TileID.Traps)
                {
                    tile.TileFrameNumber = tileData.TileFrame;
                    tile.TileFrameY = tileData.TileY;
                    tile.TileFrameX = tileData.TileX;
                }
                tile.Slope = tileData.TileSlope;
                tile.IsHalfBlock = tileData.HalfTile;
                tile.TileColor = tileData.TileColor;
                tile.WallColor = tileData.TileWallColor;
                NetMessage.SendTileSquare(-1, (int)vector2.X, (int)vector2.Y, 1);
            }
        }

        public static void Restore2(ushort MainTileID, ushort GrassTileID, Vector2 vector2)
        {
            WorldGen.PlaceTile((int)vector2.X, (int)vector2.Y, MainTileID, false, true);
            WorldGen.PlaceTile((int)vector2.X, (int)vector2.Y, GrassTileID, false, true);
        }
    }
}
