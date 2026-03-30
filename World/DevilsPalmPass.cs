using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using Terraria.IO;

namespace JoJoStands
{
    public class DevilsPalmPass : GenPass
    {
        private const int BiomeWidth = 400;
        private const int SpikeCount = 8;
        private const int CraterWidth = 120;
        private const int CraterDepth = 55;
        private const int SpikeHeightMin = 35;
        private const int SpikeHeightMax = 75;
        private const int SpikeBaseWidth = 18;
        private const int BlendMargin = 60;

        private const int MinBuildings = 6;
        private const int MaxBuildings = 12;

        public DevilsPalmPass() : base("Devil's Palm", 100f) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration config)
        {
            progress.Message = "Shaping the Devil's Palm...";

            int worldSurface = (int)Main.worldSurface;

            int spawnX = Main.spawnTileX;
            int dungeonX = Main.dungeonX;

            int startX = PickPlacementX(spawnX, dungeonX);
            if (startX < 0) return;

            int endX = startX + BiomeWidth;
            int centerX = startX + BiomeWidth / 2;

            float[] baseHeights = SampleSurface(startX, BiomeWidth, worldSurface);

            float[] targetHeights = BuildTargetHeights(baseHeights, BiomeWidth, worldSurface);

            FillBiomeTiles(startX, endX, targetHeights, worldSurface);

            PlaceSurfaceDetails(startX, endX, targetHeights);

            progress.Message = "Burying the Western City...";
            GenerateUndergroundCity(startX, endX, targetHeights);

            progress.Message = "Adding surface details...";
            PlaceSurfaceDetails(startX, endX, targetHeights);
        }

        private int PickPlacementX(int spawnX, int dungeonX)
        {
            int margin = 200;
            int w = Main.maxTilesX;

            for (int attempt = 0; attempt < 40; attempt++)
            {
                int x = WorldGen.genRand.Next(w / 2, w - margin - BiomeWidth);

                bool nearSpawn = Math.Abs(x + BiomeWidth / 2 - spawnX) < BiomeWidth;
                bool nearDungeon = Math.Abs(x + BiomeWidth / 2 - dungeonX) < BiomeWidth + 100;
                if (nearSpawn || nearDungeon) continue;

                int desertScore = 0;
                for (int sx = x; sx < x + BiomeWidth; sx += 10)
                {
                    int sy = FindSurface(sx, (int)Main.worldSurface);
                    if (sy < 0) continue;
                    ushort t = Main.tile[sx, sy].TileType;
                    if (t == TileID.Sand || t == TileID.Sandstone || t == TileID.HardenedSand)
                        desertScore++;
                }
                if (desertScore >= 5) return x;
            }

            return WorldGen.genRand.Next(w / 2, w - 300 - BiomeWidth);
        }

        private float[] SampleSurface(int startX, int width, int worldSurface)
        {
            float[] h = new float[width];
            for (int i = 0; i < width; i++)
            {
                int sy = FindSurface(startX + i, worldSurface);
                h[i] = sy >= 0 ? sy : worldSurface;
            }
            return h;
        }

        private static int FindSurface(int x, int startY)
        {
            if (x < 0 || x >= Main.maxTilesX) return startY;
            for (int y = 20; y < startY + 80; y++)
            {
                if (InBounds(x, y) && Main.tile[x, y].HasTile)
                    return y;
            }
            return startY;
        }

        private float[] BuildTargetHeights(float[] baseHeights, int width, int worldSurface)
        {
            float[] target = new float[width];

            float[] sorted = (float[])baseHeights.Clone();
            Array.Sort(sorted);
            float floorY = sorted[sorted.Length / 2];

            if (floorY < worldSurface - 60) floorY = worldSurface - 20;

            int craterCenter = width / 2;
            int craterHalf = CraterWidth / 2;

            int leftZoneEnd = craterCenter - craterHalf - 20;
            int rightZoneStart = craterCenter + craterHalf + 20;

            List<int> spikeCenters = PickSpikeCenters(width, leftZoneEnd, rightZoneStart);

            for (int i = 0; i < width; i++)
                target[i] = floorY;

            for (int i = 0; i < width; i++)
            {
                float rel = (i - craterCenter) / (float)craterHalf;
                if (Math.Abs(rel) <= 1f)
                {
                    float dip = (float)(0.5 * (1 - Math.Cos(rel * Math.PI)));
                    float craterY = floorY + CraterDepth * (1f - dip);
                    target[i] = Math.Max(target[i], craterY);
                    target[i] = craterY;
                }
            }

            for (int i = 0; i < width; i++)
            {
                float rel = (i - craterCenter) / (float)craterHalf;
                if (Math.Abs(rel) > 1f)
                    target[i] = floorY;
            }

            foreach (int sc in spikeCenters)
            {
                int spikeH = WorldGen.genRand.Next(SpikeHeightMin, SpikeHeightMax + 1);
                int baseW = WorldGen.genRand.Next(SpikeBaseWidth - 4, SpikeBaseWidth + 6);

                for (int i = Math.Max(0, sc - baseW); i < Math.Min(width, sc + baseW); i++)
                {
                    float dist = Math.Abs(i - sc) / (float)baseW;
                    float spikeContrib = (float)(Math.Pow(1f - dist, 3.5) * spikeH);
                    float newY = floorY - spikeContrib;
                    if (newY < target[i])
                        target[i] = newY;
                }
            }

            for (int i = 0; i < BlendMargin; i++)
            {
                float t = i / (float)BlendMargin;
                t = t * t * (3f - 2f * t);
                target[i] = Lerp(baseHeights[i], target[i], t);
                int ri = width - 1 - i;
                target[ri] = Lerp(baseHeights[ri], target[ri], t);
            }

            return target;
        }

        private List<int> PickSpikeCenters(int width, int leftZoneEnd, int rightZoneStart)
        {
            var centers = new List<int>();
            int leftCount = SpikeCount / 2;
            int rightCount = SpikeCount - leftCount;

            if (leftZoneEnd > 30)
            {
                int spacing = Math.Max(1, leftZoneEnd / (leftCount + 1));
                for (int k = 1; k <= leftCount; k++)
                {
                    int cx = spacing * k + WorldGen.genRand.Next(-spacing / 4, spacing / 4 + 1);
                    cx = Math.Clamp(cx, 10, leftZoneEnd - 10);
                    centers.Add(cx);
                }
            }

            int rightZoneLen = width - rightZoneStart;
            if (rightZoneLen > 30)
            {
                int spacing = Math.Max(1, rightZoneLen / (rightCount + 1));
                for (int k = 1; k <= rightCount; k++)
                {
                    int cx = rightZoneStart + spacing * k + WorldGen.genRand.Next(-spacing / 4, spacing / 4 + 1);
                    cx = Math.Clamp(cx, rightZoneStart + 10, width - 10);
                    centers.Add(cx);
                }
            }

            return centers;
        }

        private void ApplyHeightMap(int startX, float[] target, float[] original, int worldSurface)
        {
            int width = target.Length;

            for (int i = 0; i < width; i++)
            {
                int x = startX + i;
                int newTop = (int)Math.Round(target[i]);
                int oldTop = (int)Math.Round(original[i]);

                if (newTop < oldTop)
                {
                    for (int y = newTop; y <= oldTop; y++)
                        ClearTile(x, y);
                }
                else if (newTop > oldTop)
                {
                    for (int y = oldTop; y <= newTop; y++)
                        FillTile(x, y);
                }
            }
        }

        private static void ClearTile(int x, int y)
        {
            if (!InBounds(x, y)) return;
            Tile t = Main.tile[x, y];
            t.HasTile = false;
            t.TileType = 0;
            t.LiquidAmount = 0;
        }

        private static void FillTile(int x, int y)
        {
            if (!InBounds(x, y)) return;
            Tile t = Main.tile[x, y];
            t.HasTile = true;
            t.TileType = TileID.HardenedSand;
        }

        private void FillBiomeTiles(int startX, int endX, float[] targetHeights, int worldSurface)
        {
            int width = endX - startX;
            int depth = 300;
            int blendRadius = 40;

            for (int i = 0; i < width; i++)
            {
                int x = startX + i;
                int surfY = (int)Math.Round(targetHeights[i]);
                int rockY = surfY + WorldGen.genRand.Next(8, 16);
                int bottomY = surfY + depth;

                for (int y = 0; y < surfY; y++)
                {
                    if (!InBounds(x, y)) continue;
                    Tile tile = Main.tile[x, y];
                    tile.HasTile = false;
                    tile.LiquidAmount = 0;
                    tile.WallType = 0;
                }

                for (int y = surfY; y < bottomY && y < Main.maxTilesY - 10; y++)
                {
                    if (!InBounds(x, y)) continue;

                    int distLeft = i;
                    int distRight = width - 1 - i;
                    int distBottom = bottomY - y;

                    int minDist = Math.Min(Math.Min(distLeft, distRight), distBottom);

                    if (minDist < blendRadius)
                    {
                        float placeChance = (float)minDist / blendRadius;
                        placeChance += WorldGen.genRand.NextFloat(-0.1f, 0.1f);
                        if (WorldGen.genRand.NextFloat() > placeChance)
                        {
                            continue;
                        }
                    }

                    ushort tileType;
                    if (y < rockY)
                        tileType = TileID.HardenedSand;
                    else if (y < rockY + 20)
                        tileType = TileID.Sandstone;
                    else
                        tileType = TileID.HardenedSand;

                    Tile tile = Main.tile[x, y];
                    tile.HasTile = true;
                    tile.TileType = tileType;
                    tile.LiquidAmount = 0;
                    tile.WallType = WallID.HardenedSand;
                }
            }
        }

        private void GenerateUndergroundCity(int startX, int endX, float[] targetHeights)
        {
            int numBuildings = WorldGen.genRand.Next(MinBuildings, MaxBuildings + 1);
            int width = endX - startX;

            for (int i = 0; i < numBuildings; i++)
            {
                int bX = startX + WorldGen.genRand.Next(40, width - 40);
                int surfY = (int)targetHeights[bX - startX];
                int bY = surfY + WorldGen.genRand.Next(40, 200);
                int bWidth = WorldGen.genRand.Next(18, 35);
                int bHeight = WorldGen.genRand.Next(12, 25);
                BuildWesternHouse(bX, bY, bWidth, bHeight);
            }
        }

        private void BuildWesternHouse(int x, int y, int width, int height)
        {
            ushort frameTile = TileID.WoodBlock;
            ushort bgWall = WallID.Wood;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int curX = x + i;
                    int curY = y + j;

                    if (!InBounds(curX, curY)) continue;

                    Tile t = Main.tile[curX, curY];
                    t.ClearEverything();
                    WorldGen.PlaceWall(curX, curY, bgWall, mute: true);
                    if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
                        WorldGen.PlaceTile(curX, curY, frameTile, mute: true, forced: true);
                }
            }

            int doorX = x;
            int doorY = y + height - 2;

            if (InBounds(doorX, doorY) && InBounds(doorX, doorY - 1) && InBounds(doorX, doorY - 2))
            {
                Main.tile[doorX, doorY].ClearEverything();
                Main.tile[doorX, doorY - 1].ClearEverything();
                Main.tile[doorX, doorY - 2].ClearEverything();

                WorldGen.PlaceWall(doorX, doorY, bgWall, mute: true);
                WorldGen.PlaceWall(doorX, doorY - 1, bgWall, mute: true);
                WorldGen.PlaceWall(doorX, doorY - 2, bgWall, mute: true);

                WorldGen.PlaceTile(doorX, doorY, TileID.ClosedDoor, mute: true, forced: true);
            }

            int chestX = x + width / 2;
            int chestY = y + height - 2;

            int chestIndex = WorldGen.PlaceChest(chestX, chestY, 21, false, 0);
            if (chestIndex >= 0 && chestIndex < Main.maxChests)
            {
                Chest chest = Main.chest[chestIndex];

                chest.item[0].SetDefaults(ItemID.GoldCoin);
                chest.item[0].stack = WorldGen.genRand.Next(5, 15);

            }
            WorldGen.PlaceTile(x + 2, y + height - 4, TileID.Torches, mute: true, forced: true);
            WorldGen.PlaceTile(x + width - 3, y + height - 4, TileID.Torches, mute: true, forced: true);
        }

        private void PlaceSurfaceDetails(int startX, int endX, float[] targetHeights)
        {
            int width = endX - startX;

            for (int i = 2; i < width - 2; i++)
            {
                int x = startX + i;
                int surfY = (int)Math.Round(targetHeights[i]) - 1;

                if (!InBounds(x, surfY)) continue;

                if (WorldGen.genRand.Next(35) == 0)
                {
                    int belowY = surfY + 1;
                    if (InBounds(x, belowY) && Main.tile[x, belowY].HasTile)
                        WorldGen.PlaceTile(x, surfY, TileID.Cactus, true, true);
                }

                if (WorldGen.genRand.Next(20) == 0)
                {
                    WorldGen.TileRunner(x, surfY + 1, WorldGen.genRand.Next(2, 5), 2,
                        TileID.Sand, addTile: false, overRide: false);
                }
            }
        }

        private static float Lerp(float a, float b, float t) => a + (b - a) * t;

        private static bool InBounds(int x, int y) =>
            x >= 0 && x < Main.maxTilesX && y >= 0 && y < Main.maxTilesY;
    }
}