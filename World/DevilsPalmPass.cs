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
        // ─── Tuning constants ────────────────────────────────────────────────────
        private const int BiomeWidth = 400;   // total horizontal span (tiles)
        private const int SpikeCount = 8;     // number of sandstone spikes
        private const int CraterWidth = 120;   // crater diameter (tiles)
        private const int CraterDepth = 55;    // how deep the crater dips
        private const int SpikeHeightMin = 35;
        private const int SpikeHeightMax = 75;
        private const int SpikeBaseWidth = 18;    // half-width of a spike base
        private const int BlendMargin = 60;    // tiles over which height blends to neighbours

        public DevilsPalmPass() : base("Devil's Palm", 100f) { }

        // ─── Entry point ─────────────────────────────────────────────────────────
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration config)
        {
            progress.Message = "Shaping the Devil's Palm...";

            int worldSurface = (int)Main.worldSurface;

            // Pick a placement zone: right third of the map, away from spawn and dungeon
            int spawnX = Main.spawnTileX;
            int dungeonX = Main.dungeonX;

            // Try to find a flat-ish desert area in the right portion of the map
            int startX = PickPlacementX(spawnX, dungeonX);
            if (startX < 0) return; // couldn't find suitable space

            int endX = startX + BiomeWidth;
            int centerX = startX + BiomeWidth / 2;

            // --- Build height map ---------------------------------------------------
            // 1. Sample the existing vanilla surface heights across the biome width
            float[] baseHeights = SampleSurface(startX, BiomeWidth, worldSurface);

            // 2. Compute the "target" heights: flat desert + spikes + crater
            float[] targetHeights = BuildTargetHeights(baseHeights, BiomeWidth, worldSurface);

            // 3. Apply blended height map to the world

            // --- Fill with biome tiles -----------------------------------------------
            FillBiomeTiles(startX, endX, targetHeights, worldSurface);

            // --- Ore / detail passes -------------------------------------------------
            PlaceSurfaceDetails(startX, endX, targetHeights);
        }

        // ─── Step 1 – pick placement X ───────────────────────────────────────────
        private int PickPlacementX(int spawnX, int dungeonX)
        {
            int margin = 200; // stay away from world edges
            int w = Main.maxTilesX;

            // Build list of candidate start positions, biased toward right side
            for (int attempt = 0; attempt < 40; attempt++)
            {
                int x = WorldGen.genRand.Next(w / 2, w - margin - BiomeWidth);

                // Don't overlap spawn or dungeon areas
                bool nearSpawn = Math.Abs(x + BiomeWidth / 2 - spawnX) < BiomeWidth;
                bool nearDungeon = Math.Abs(x + BiomeWidth / 2 - dungeonX) < BiomeWidth + 100;
                if (nearSpawn || nearDungeon) continue;

                // Prefer sandy / desert surface tiles
                int desertScore = 0;
                for (int sx = x; sx < x + BiomeWidth; sx += 10)
                {
                    int sy = FindSurface(sx, (int)Main.worldSurface);
                    if (sy < 0) continue;
                    ushort t = Main.tile[sx, sy].TileType;
                    if (t == TileID.Sand || t == TileID.Sandstone || t == TileID.HardenedSand)
                        desertScore++;
                }
                if (desertScore >= 5) return x; // good enough
            }

            // Fallback: just pick a safe x on the right side
            return WorldGen.genRand.Next(w / 2, w - 300 - BiomeWidth);
        }

        // ─── Step 2 – sample vanilla surface ─────────────────────────────────────
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

        // Returns the Y of the first solid tile at column x, searching from above
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

        // ─── Step 3 – build target height map ────────────────────────────────────
        private float[] BuildTargetHeights(float[] baseHeights, int width, int worldSurface)
        {
            float[] target = new float[width];

            // Use the median of the sampled heights as the "flat desert floor"
            float[] sorted = (float[])baseHeights.Clone();
            Array.Sort(sorted);
            float floorY = sorted[sorted.Length / 2];

            // Clamp floor to something sensible
            if (floorY < worldSurface - 60) floorY = worldSurface - 20;

            int craterCenter = width / 2;
            int craterHalf = CraterWidth / 2;

            // --- Place spikes -------------------------------------------------------
            // Spikes occupy left zone [0 .. craterCenter-craterHalf-20]
            // and right zone [craterCenter+craterHalf+20 .. width]
            // Exclude a 20-tile "ramp" approach to the crater on each side

            int leftZoneEnd = craterCenter - craterHalf - 20;
            int rightZoneStart = craterCenter + craterHalf + 20;

            List<int> spikeCenters = PickSpikeCenters(width, leftZoneEnd, rightZoneStart);

            // Base height is flat desert floor everywhere first
            for (int i = 0; i < width; i++)
                target[i] = floorY;

            // Carve crater (smooth parabola)
            for (int i = 0; i < width; i++)
            {
                float rel = (i - craterCenter) / (float)craterHalf; // -1..1
                if (Math.Abs(rel) <= 1f)
                {
                    // Smooth cosine crater profile
                    float dip = (float)(0.5 * (1 - Math.Cos(rel * Math.PI)));
                    float craterY = floorY + CraterDepth * (1f - dip);
                    target[i] = Math.Max(target[i], craterY); // deeper = larger Y
                    // actually crater dips DOWN (larger Y = deeper)
                    target[i] = craterY;
                }
            }

            // Re-flatten outside crater
            for (int i = 0; i < width; i++)
            {
                float rel = (i - craterCenter) / (float)craterHalf;
                if (Math.Abs(rel) > 1f)
                    target[i] = floorY;
            }

            // Add spikes (sharp exponential peaks, like the reference)
            foreach (int sc in spikeCenters)
            {
                int spikeH = WorldGen.genRand.Next(SpikeHeightMin, SpikeHeightMax + 1);
                int baseW = WorldGen.genRand.Next(SpikeBaseWidth - 4, SpikeBaseWidth + 6);

                for (int i = Math.Max(0, sc - baseW); i < Math.Min(width, sc + baseW); i++)
                {
                    float dist = Math.Abs(i - sc) / (float)baseW; // 0 at peak, 1 at base
                    // Sharp exponential taper (matches reference image)
                    float spikeContrib = (float)(Math.Pow(1f - dist, 3.5) * spikeH);
                    float newY = floorY - spikeContrib;
                    if (newY < target[i])
                        target[i] = newY;
                }
            }

            // Blend left and right edges into neighbour terrain
            for (int i = 0; i < BlendMargin; i++)
            {
                float t = i / (float)BlendMargin;
                // smooth-step
                t = t * t * (3f - 2f * t);
                // left edge
                target[i] = Lerp(baseHeights[i], target[i], t);
                // right edge
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

            // Left spikes
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

            // Right spikes
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

        // ─── Step 4 – carve / fill world tiles to match height map ───────────────
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
                    // Need to remove tiles above newTop (spike going up)
                    for (int y = newTop; y <= oldTop; y++)
                        ClearTile(x, y);
                }
                else if (newTop > oldTop)
                {
                    // Need to fill tiles down to newTop (crater / depression)
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
            int depth = 300; // How far down the biome goes
            int blendRadius = 40; // The thickness of the fuzzy/jagged edge transition

            for (int i = 0; i < width; i++)
            {
                int x = startX + i;
                int surfY = (int)Math.Round(targetHeights[i]);
                int rockY = surfY + WorldGen.genRand.Next(8, 16);
                int bottomY = surfY + depth;

                // 1. Clear EVERYTHING above the new surface (fixes floating vanilla terrain)
                for (int y = 0; y < surfY; y++)
                {
                    if (!InBounds(x, y)) continue;
                    Tile tile = Main.tile[x, y];
                    tile.HasTile = false;
                    tile.LiquidAmount = 0;
                    tile.WallType = 0;
                }

                // 2. Fill solid tiles downward with edge blending
                for (int y = surfY; y < bottomY && y < Main.maxTilesY - 10; y++)
                {
                    if (!InBounds(x, y)) continue;

                    // --- EDGE BLENDING MATH ---
                    // Calculate distance to the left, right, and bottom edges
                    int distLeft = i;
                    int distRight = width - 1 - i;
                    int distBottom = bottomY - y;

                    // Find the shortest distance to ANY edge
                    int minDist = Math.Min(Math.Min(distLeft, distRight), distBottom);

                    // If we are within the blend radius, introduce a random chance to skip placement
                    if (minDist < blendRadius)
                    {
                        // Convert distance to a percentage (0.0 to 1.0)
                        float placeChance = (float)minDist / blendRadius;

                        // Add a tiny bit of noise to make the gradient less perfectly smooth
                        placeChance += WorldGen.genRand.NextFloat(-0.1f, 0.1f);

                        // If the random roll beats our chance, skip placing our tile 
                        // and let the vanilla terrain/cave remain intact.
                        if (WorldGen.genRand.NextFloat() > placeChance)
                        {
                            continue;
                        }
                    }

                    // --- TILE PLACEMENT ---
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

        // ─── Step 6 – surface decoration ─────────────────────────────────────────
        private void PlaceSurfaceDetails(int startX, int endX, float[] targetHeights)
        {
            int width = endX - startX;

            for (int i = 2; i < width - 2; i++)
            {
                int x = startX + i;
                int surfY = (int)Math.Round(targetHeights[i]) - 1; // one above surface

                if (!InBounds(x, surfY)) continue;

                // Small cacti / antlion holes scattered on flat sections
                if (WorldGen.genRand.Next(35) == 0)
                {
                    int belowY = surfY + 1;
                    if (InBounds(x, belowY) && Main.tile[x, belowY].HasTile)
                        WorldGen.PlaceTile(x, surfY, TileID.Cactus, true, true);
                }

                // Sand piles
                if (WorldGen.genRand.Next(20) == 0)
                {
                    WorldGen.TileRunner(x, surfY + 1, WorldGen.genRand.Next(2, 5), 2,
                        TileID.Sand, addTile: false, overRide: false);
                }
            }
        }

        // ─── Utilities ───────────────────────────────────────────────────────────
        private static float Lerp(float a, float b, float t) => a + (b - a) * t;

        private static bool InBounds(int x, int y) =>
            x >= 0 && x < Main.maxTilesX && y >= 0 && y < Main.maxTilesY;
    }
}