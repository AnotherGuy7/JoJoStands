using JoJoStands.DataStructures;
using JoJoStands.Items;
using JoJoStands.Networking;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace JoJoStands
{
    public class JoJoStandsWorld : ModSystem
    {
        private bool viralMeteoriteDropped = false;
        private bool vampiricNightQueued = false;
        private bool checkedForVampiricEvent = false;
        private bool vampiricNightComplete = false;
        private int vampiricNightStartTimer = 0;
        private int viralMeteoriteIntroductionTimer = 0;

        public static bool VisitedViralMeteorite = false;
        public static bool ViralMeteoriteIntroduced = false;
        public static bool VampiricNight = false;
        public static Point ViralMeteoriteCenter;
        public static int viralMeteoriteTiles = 0;

        private const string Tag_MeteorDropped = "meteorDropped";
        private const string Tag_VampiricNight = "vampiricNight";
        private const string Tag_VampiricNightComplete = "vampiricNightComplete";
        private const string Tag_ViralMeteoriteCenterX = "viralMeteoriteCenterX";
        private const string Tag_ViralMeteoriteCenterY = "viralMeteoriteCenterY";
        private const string Tag_ViralMeteoriteVisited = "viralMeteoriteVisited";
        private const string Tag_ViralMeteoriteIntroZoneVisited = "viralMeteoriteIntroduced";
        public static readonly Color WorldEventTextColor = new Color(50, 255, 130);

        public override void OnWorldLoad()
        {
            ResetWorldDependentVariables();
        }

        public override void ClearWorld()
        {
            ResetWorldDependentVariables();
        }

        public override void OnWorldUnload()
        {
            ResetWorldDependentVariables();
        }

        private void ResetWorldDependentVariables()
        {
            viralMeteoriteDropped = false;
            vampiricNightQueued = false;
            checkedForVampiricEvent = false;
            vampiricNightComplete = false;
            vampiricNightStartTimer = 0;
            viralMeteoriteIntroductionTimer = 0;

            VisitedViralMeteorite = false;
            ViralMeteoriteIntroduced = false;
            VampiricNight = false;
            ViralMeteoriteCenter = Point.Zero;
            viralMeteoriteTiles = 0;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add(Tag_MeteorDropped, viralMeteoriteDropped);
            tag.Add(Tag_VampiricNight, VampiricNight);
            tag.Add(Tag_VampiricNightComplete, vampiricNightComplete);
            tag.Add(Tag_ViralMeteoriteCenterX, ViralMeteoriteCenter.X);
            tag.Add(Tag_ViralMeteoriteCenterY, ViralMeteoriteCenter.Y);
            tag.Add(Tag_ViralMeteoriteVisited, VisitedViralMeteorite);
            tag.Add(Tag_ViralMeteoriteIntroZoneVisited, ViralMeteoriteIntroduced);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            viralMeteoriteDropped = tag.GetBool(Tag_MeteorDropped);
            VampiricNight = tag.GetBool(Tag_VampiricNight);
            vampiricNightComplete = tag.GetBool(Tag_VampiricNightComplete);
            ViralMeteoriteCenter = new Point(tag.GetInt(Tag_ViralMeteoriteCenterX), tag.GetInt(Tag_ViralMeteoriteCenterY));
            VisitedViralMeteorite = tag.GetBool(Tag_ViralMeteoriteVisited);
            ViralMeteoriteIntroduced = tag.GetBool(Tag_ViralMeteoriteIntroZoneVisited);
        }

        public override void PreUpdateWorld()
        {
            if (NPC.downedBoss3 && !viralMeteoriteDropped && Main.dayTime)
            {
                DropViralMeteorite();
                viralMeteoriteDropped = true;
            }

            if (viralMeteoriteDropped && !VisitedViralMeteorite)
            {
                if (Math.Abs(ViralMeteoriteCenter.X - Main.player[Main.myPlayer].Center.X) <= 290 * 16)
                {
                    JoJoStandsShaders.ActivateShader(JoJoStandsShaders.ViralMeteoriteEffect);
                    if (viralMeteoriteIntroductionTimer < 5 * 60)
                        viralMeteoriteIntroductionTimer++;

                    JoJoStandsShaders.ChangeShaderUseProgress(JoJoStandsShaders.ViralMeteoriteEffect, 0.6f * (viralMeteoriteIntroductionTimer / (5f * 60f)));
                    if (!ViralMeteoriteIntroduced)
                    {
                        ViralMeteoriteIntroduced = true;
                        Main.NewText("A faint spiritual connection can be sensed nearby.", WorldEventTextColor);
                    }
                }
                else
                {
                    viralMeteoriteIntroductionTimer = 0;
                    JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.ViralMeteoriteEffect);
                }
            }

            if (Main.netMode == NetmodeID.MultiplayerClient && Main.myPlayer != 0)      //Below this, the code only updates for the owner.
                return;

            if (!Main.dayTime)
            {
                if (!checkedForVampiricEvent && !vampiricNightComplete && NPC.downedBoss1 && !VampiricNight)
                {
                    checkedForVampiricEvent = true;
                    if (Main.rand.Next(1, 100 + 1) <= 9)
                    {
                        vampiricNightQueued = true;
                        SyncCall.DisplayText("You feel the dirt under you rumbling slightly...", WorldEventTextColor);
                    }
                }
                if (vampiricNightQueued)
                {
                    vampiricNightStartTimer++;
                    if (vampiricNightStartTimer >= 20 * 60)
                    {
                        VampiricNight = true;
                        vampiricNightQueued = false;
                        vampiricNightStartTimer = 0;
                        SyncCall.DisplayText("Dio's Minions have arrived!", WorldEventTextColor);
                        SyncCall.SyncVampiricNight(Main.myPlayer, VampiricNight);
                    }
                }
            }
            else
            {
                checkedForVampiricEvent = false;
                if (VampiricNight)
                {
                    VampiricNight = false;
                    vampiricNightComplete = true;
                    Main.NewText("The zombies have been pushed back!", WorldEventTextColor);
                }
            }
        }

        public override void PostWorldGen()     //once again, from ExampleMod
        {
            int[] itemsToPlaceInIceChests = { ModContent.ItemType<RustyRevolver>() };
            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 1 * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)     //40 is the max amount of items a chest can hold
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            if (Main.rand.Next(0, 100 + 1) <= 40)
                                chest.item[inventoryIndex].SetDefaults(itemsToPlaceInIceChests[0]);
                            break;
                        }
                    }
                }
            }
        }

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            viralMeteoriteTiles = tileCounts[ModContent.TileType<ViralMeteoriteTile>()];
        }

        public static void DropViralMeteorite()        //directly from Terraria/WorldGen.cs, about 22~~
        {
            bool droppedMeteorite = true;
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active)
                {
                    droppedMeteorite = false;
                    break;
                }
            }

            int iterations = 0;
            int chosenMeteoriteDepth = (int)(400f * (float)(Main.maxTilesX / 4200));
            for (int xCoord = 5; xCoord < Main.maxTilesX - 5; xCoord++)
            {
                int yCoord = 5;
                while ((double)yCoord < Main.worldSurface)
                {
                    if (Main.tile[xCoord, yCoord].HasTile && Main.tile[xCoord, yCoord].TileType == ModContent.TileType<ViralMeteoriteTile>())
                    {
                        iterations++;
                        if (iterations > chosenMeteoriteDepth)
                            return;
                    }
                    yCoord++;
                }
            }

            float lowestTCoordinate = 600f;
            while (!droppedMeteorite)
            {
                float spawnAreaDistance = (float)Main.maxTilesX * 0.08f;        //Section of the world that's considered "spawn area"
                int xCoord = Main.rand.Next(150, Main.maxTilesX - 150);
                while ((float)xCoord > (float)Main.spawnTileX - spawnAreaDistance && (float)xCoord < (float)Main.spawnTileX + spawnAreaDistance)        //When the chosen coordinate isn't near spawn
                {
                    xCoord = Main.rand.Next(150, Main.maxTilesX - 150);
                }

                int yCoord = (int)(Main.worldSurface * 0.3);
                while (yCoord < Main.maxTilesY)
                {
                    if (Main.tile[xCoord, yCoord].HasTile && Main.tileSolid[(int)Main.tile[xCoord, yCoord].TileType])
                    {
                        int yLevel = 0;
                        int meteoriteArea = 15;        //Size of the meteor as a radius
                        for (int x = xCoord - meteoriteArea; x < xCoord + meteoriteArea; x++)
                        {
                            for (int y = yCoord - meteoriteArea; y < yCoord + meteoriteArea; y++)
                            {
                                if (WorldGen.SolidTile(x, y))
                                {
                                    yLevel++;
                                    if (Main.tile[x, y].TileType == TileID.Cloud || Main.tile[x, y].TileType == TileID.Sunplate)
                                        yLevel -= 100;
                                }
                                else if (Main.tile[x, y].LiquidAmount > 0)
                                {
                                    yLevel--;
                                }
                            }
                        }
                        if ((float)yLevel < lowestTCoordinate)
                        {
                            lowestTCoordinate -= 0.5f;
                            break;
                        }
                        droppedMeteorite = GenereateViralMeteorite(xCoord, yCoord);
                        if (droppedMeteorite)
                            break;

                        break;
                    }
                    else
                    {
                        yCoord++;
                    }
                }
                if (lowestTCoordinate < 100f)
                    return;
            }
        }

        public static bool GenereateViralMeteorite(int x, int y)     //from WorldGen.cs, line 2303
        {
            int worldBoundaries = 50;
            if (x < worldBoundaries || x > Main.maxTilesX - worldBoundaries || y < worldBoundaries || y > Main.maxTilesY - worldBoundaries)
                return false;

            //Checks to make sure the meteorite can generate in this area
            int meteoriteArea = 35;         //The max amount of space the VM can take up
            Rectangle meteoriteRect = new Rectangle((x - meteoriteArea) * 16, (y - meteoriteArea) * 16, meteoriteArea * 2 * 16, meteoriteArea * 2 * 16);
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];
                if (player.active)
                {
                    Rectangle playerArea = new Rectangle((int)(player.position.X + (float)(player.width / 2) - (float)(NPC.sWidth / 2) - (float)NPC.safeRangeX), (int)(player.position.Y + (float)(player.height / 2) - (float)(NPC.sHeight / 2) - (float)NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    if (meteoriteRect.Intersects(playerArea))
                        return false;
                }
            }
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    Rectangle npcArea = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);         //How much space that NPC is taking up and where
                    if (meteoriteRect.Intersects(npcArea))
                        return false;
                }
            }
            for (int xCoord = x - meteoriteArea; xCoord < x + meteoriteArea; xCoord++)
            {
                for (int yCoord = y - meteoriteArea; yCoord < y + meteoriteArea; yCoord++)
                {
                    if (Main.tile[xCoord, yCoord].HasTile && TileID.Sets.BasicChest[(int)Main.tile[xCoord, yCoord].TileType])
                        return false;
                }
            }

            meteoriteArea = WorldGen.genRand.Next(17, 23);
            for (int xCoord = x - meteoriteArea; xCoord < x + meteoriteArea; xCoord++)
            {
                for (int yCoord = y - meteoriteArea; yCoord < y + meteoriteArea; yCoord++)
                {
                    if (yCoord > y + Main.rand.Next(-2, 3) - 5)     //Deciding random Y placement
                    {
                        int centerDisplacementX = Math.Abs(x - xCoord);
                        int centerDisplacementY = Math.Abs(y - yCoord);
                        float tileDistance = (float)Math.Sqrt((double)(centerDisplacementX * centerDisplacementX + centerDisplacementY * centerDisplacementY));
                        if ((double)tileDistance < (double)meteoriteArea * 0.9 + (double)Main.rand.Next(-4, 4 + 1))
                        {
                            if (!Main.tileSolid[(int)Main.tile[xCoord, yCoord].TileType])
                                Main.tile[xCoord, yCoord].ClearTile();

                            Main.tile[xCoord, yCoord].TileType = (ushort)ModContent.TileType<ViralMeteoriteTile>();
                        }
                    }
                }
            }
            meteoriteArea = WorldGen.genRand.Next(8, 14);
            for (int xCoord = x - meteoriteArea; xCoord < x + meteoriteArea; xCoord++)
            {
                for (int yCoord = y - meteoriteArea; yCoord < y + meteoriteArea; yCoord++)
                {
                    if (yCoord > y + Main.rand.Next(-2, 3) - 4)
                    {
                        int centerDisplacementX = Math.Abs(x - xCoord);
                        int centerDisplacementY = Math.Abs(y - yCoord);
                        float tileDistance = (float)Math.Sqrt((double)(centerDisplacementX * centerDisplacementX + centerDisplacementY * centerDisplacementY));
                        if ((double)tileDistance < (double)meteoriteArea * 0.8 + (double)Main.rand.Next(-3, 4))
                            Main.tile[xCoord, yCoord].ClearTile();
                    }
                }
            }
            meteoriteArea = WorldGen.genRand.Next(25, 35);
            for (int xCoord = x - meteoriteArea; xCoord < x + meteoriteArea; xCoord++)
            {
                for (int yCoord = y - meteoriteArea; yCoord < y + meteoriteArea; yCoord++)
                {
                    int centerDisplacementX = Math.Abs(x - xCoord);
                    int centerDisplacementY = Math.Abs(y - yCoord);
                    float tileDistance = (float)Math.Sqrt((double)(centerDisplacementX * centerDisplacementX + centerDisplacementY * centerDisplacementY));
                    if ((double)tileDistance < (double)meteoriteArea * 0.7)
                    {
                        if (Main.tile[xCoord, yCoord].TileType == TileID.Trees || Main.tile[xCoord, yCoord].TileType == TileID.CorruptThorns || Main.tile[xCoord, yCoord].TileType == TileID.CrimsonThorns)
                            WorldGen.KillTile(xCoord, yCoord, false, false, false);

                        Main.tile[xCoord, yCoord].LiquidAmount = 0;
                    }
                    if (Main.tile[xCoord, yCoord].TileType == ModContent.TileType<ViralMeteoriteTile>())
                    {
                        if (!WorldGen.SolidTile(xCoord - 1, yCoord) && !WorldGen.SolidTile(xCoord + 1, yCoord) && !WorldGen.SolidTile(xCoord, yCoord - 1) && !WorldGen.SolidTile(xCoord, yCoord + 1))
                            Main.tile[xCoord, yCoord].ClearTile();

                        else if ((Main.tile[xCoord, yCoord].IsHalfBlock || Main.tile[xCoord - 1, yCoord].TopSlope) && !WorldGen.SolidTile(xCoord, yCoord + 1))
                            Main.tile[xCoord, yCoord].ClearTile();
                    }
                    WorldGen.SquareTileFrame(xCoord, yCoord, true);
                    WorldGen.SquareWallFrame(xCoord, yCoord, true);
                }
            }

            meteoriteArea = WorldGen.genRand.Next(23, 32);
            for (int xCoord = x - meteoriteArea; xCoord < x + meteoriteArea; xCoord++)
            {
                for (int yCoord = y - meteoriteArea; yCoord < y + meteoriteArea; yCoord++)
                {
                    if (yCoord > y + WorldGen.genRand.Next(-3, 4) - 3 && Main.tile[xCoord, yCoord].HasTile && Main.rand.NextBool(10))
                    {
                        int centerDisplacementX = Math.Abs(x - xCoord);
                        int centerDisplacementY = Math.Abs(y - yCoord);
                        float tileDistance = (float)Math.Sqrt((double)(centerDisplacementX * centerDisplacementX + centerDisplacementY * centerDisplacementY));
                        if ((double)tileDistance < (double)meteoriteArea * 0.8)
                        {
                            if (Main.tile[xCoord, yCoord].TileType == TileID.Trees || Main.tile[xCoord, yCoord].TileType == TileID.CorruptThorns || Main.tile[xCoord, yCoord].TileType == TileID.CrimsonThorns)
                                WorldGen.KillTile(xCoord, yCoord, false, false, false);

                            Main.tile[xCoord, yCoord].TileType = (ushort)ModContent.TileType<ViralMeteoriteTile>();
                            WorldGen.SquareTileFrame(xCoord, yCoord, true);
                        }
                    }
                }
            }
            meteoriteArea = WorldGen.genRand.Next(30, 38);
            for (int xCoord = x - meteoriteArea; xCoord < x + meteoriteArea; xCoord++)
            {
                for (int yCoord = y - meteoriteArea; yCoord < y + meteoriteArea; yCoord++)
                {
                    if (yCoord > y + WorldGen.genRand.Next(-2, 3) && Main.tile[xCoord, yCoord].HasTile && Main.rand.NextBool(20))
                    {
                        int centerDisplacementX = Math.Abs(x - xCoord);
                        int centerDisplacementY = Math.Abs(y - yCoord);
                        float tileDistance = (float)Math.Sqrt((double)(centerDisplacementX * centerDisplacementX + centerDisplacementY * centerDisplacementY));
                        if ((double)tileDistance < (double)meteoriteArea * 0.85)
                        {
                            if (Main.tile[xCoord, yCoord].TileType == TileID.Trees || Main.tile[xCoord, yCoord].TileType == TileID.CorruptThorns || Main.tile[xCoord, yCoord].TileType == TileID.CrimsonThorns)
                                WorldGen.KillTile(xCoord, yCoord, false, false, false);

                            Main.tile[xCoord, yCoord].TileType = (ushort)ModContent.TileType<ViralMeteoriteTile>();
                            WorldGen.SquareTileFrame(xCoord, yCoord, true);
                        }
                    }
                }
            }
            SyncCall.DisplayText("A dangerous virus now inhabits " + Main.worldName + "... Perhaps Jotaro the Marine Biologist may know something about it.", WorldEventTextColor);
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(-1, x, y, 40, TileChangeType.None);
            Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().awaitingViralMeteoriteTip = true;
            ViralMeteoriteCenter = new Point(x * 16, y * 16);
            return true;
        }

        public override void PreSaveAndQuit()
        {
            MyPlayer player = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            player.crazyDiamondDestroyedTileData.ForEach(DestroyedTileData.Restore);
            player.crazyDiamondMessageCooldown = 0;
            player.crazyDiamondDestroyedTileData.Clear();
        }
    }
}