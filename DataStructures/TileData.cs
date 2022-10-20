using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.DataStructures
{
    public struct DestroyedTileData         
    {
        public ushort tileType { get; set; }
        public Vector2 tilePosition { get; set; }
        public SlopeType tileSlope { get; set; }
        public bool tileHalf { get; set; }
        public short tileY { get; set; }
        public short tileX { get; set; }
        public int tileFrame { get; set; }

        public DestroyedTileData(ushort tileType, Vector2 tilePosition, SlopeType tileSlope, bool tileHalf, short tileY, short tileX, int tileFrame)
        {
            this.tileType = tileType;
            this.tilePosition = tilePosition;
            this.tileSlope = tileSlope;
            this.tileHalf = tileHalf;
            this.tileY = tileY;
            this.tileX = tileX;
            this.tileFrame = tileFrame;
        }

        public static void Destroy(DestroyedTileData ExtraTile)
        {
            Vector2 vector2 = ExtraTile.tilePosition;
            WorldGen.KillTile((int)vector2.X, (int)vector2.Y, false, false, true);
            NetMessage.SendTileSquare(-1, (int)vector2.X, (int)vector2.Y, 1);
        }

        public static void Restore(DestroyedTileData tileData)
        {
            Vector2 vector2 = tileData.tilePosition;
            Tile tile = Main.tile[(int)vector2.X, (int)vector2.Y];
            if (!tile.HasTile)
            {
                if (tileData.tileType != TileID.Grass || tileData.tileType != TileID.CorruptGrass || tileData.tileType != TileID.CrimsonGrass || tileData.tileType != TileID.HallowedGrass || tileData.tileType != TileID.JungleGrass || tileData.tileType != TileID.MushroomGrass || tileData.tileType != TileID.GolfGrass || tileData.tileType != TileID.GolfGrassHallowed)
                    WorldGen.PlaceTile((int)vector2.X, (int)vector2.Y, tileData.tileType, false, true);
                if (tileData.tileType == TileID.Grass || tileData.tileType == TileID.CorruptGrass || tileData.tileType == TileID.CrimsonGrass || tileData.tileType == TileID.HallowedGrass || tileData.tileType == TileID.GolfGrass || tileData.tileType == TileID.GolfGrassHallowed)
                    Restore2(TileID.Dirt, TileID.Grass, vector2);
                if (tileData.tileType == TileID.CorruptGrass || tileData.tileType == TileID.CrimsonGrass || tileData.tileType == TileID.HallowedGrass || tileData.tileType == TileID.GolfGrass || tileData.tileType == TileID.GolfGrassHallowed)
                    Restore2(TileID.Dirt, TileID.CorruptGrass, vector2);
                if (tileData.tileType == TileID.CrimsonGrass || tileData.tileType == TileID.HallowedGrass || tileData.tileType == TileID.GolfGrass || tileData.tileType == TileID.GolfGrassHallowed)
                    Restore2(TileID.Dirt, TileID.CrimsonGrass, vector2);
                if (tileData.tileType == TileID.HallowedGrass || tileData.tileType == TileID.GolfGrass || tileData.tileType == TileID.GolfGrassHallowed)
                    Restore2(TileID.Dirt, TileID.HallowedGrass, vector2);
                if (tileData.tileType == TileID.GolfGrass || tileData.tileType == TileID.GolfGrassHallowed)
                    Restore2(TileID.Dirt, TileID.GolfGrass, vector2);
                if (tileData.tileType == TileID.GolfGrassHallowed)
                    Restore2(TileID.Dirt, TileID.GolfGrassHallowed, vector2);
                if (tileData.tileType == TileID.JungleGrass)
                    Restore2(TileID.Mud, TileID.JungleGrass, vector2);
                if (tileData.tileType == TileID.MushroomGrass)
                    Restore2(TileID.Mud, TileID.MushroomGrass, vector2);
                if (tile.TileType == TileID.Traps)
                {
                    tile.TileFrameNumber = tileData.tileFrame;
                    tile.TileFrameY = tileData.tileY;
                    tile.TileFrameX = tileData.tileX;
                }
                tile.Slope = tileData.tileSlope;
                tile.IsHalfBlock = tileData.tileHalf;
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
