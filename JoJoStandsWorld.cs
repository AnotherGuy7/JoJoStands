using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace JoJoStands
{
    public class JoJoStandsWorld : ModWorld
    {
        private bool meteorDropped = false;
        public static int viralMeteoriteTiles = 0;

        public override void PreUpdate()
        {
            if (NPC.downedBoss3 && !meteorDropped && Main.dayTime)
            {
                DropViralMeteorite();
                meteorDropped = true;
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "meteorDropped", meteorDropped }
            };
        }

        public override void Load(TagCompound tag)
        {
            meteorDropped = tag.GetBool("meteorDropped");
        }

        public override void PostWorldGen()     ///once again, from ExampleMod
        {
            int[] itemsToPlaceInIceChests = { mod.ItemType("RustyRevolver") };
            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 1 * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)     //40 is the max amount of items a chest can hold
                    {
                        if (chest.item[inventoryIndex].type == 0)
                        {
                            if (Main.rand.NextFloat(0f, 101f) < 40f)
                            {
                                chest.item[inventoryIndex].SetDefaults(itemsToPlaceInIceChests[0]);
                            }
                            break;
                        }
                    }
                }
            }
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            viralMeteoriteTiles = tileCounts[mod.TileType("ViralMeteoriteTile")];
        }

        public static void DropViralMeteorite()        //directly from Terraria/WorldGen.cs, about 22~~
        {
            bool droppedMeteorite = true;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active)
                {
                    droppedMeteorite = false;
                    break;
                }
            }

            int num = 0;
            float num2 = (float)(Main.maxTilesX / 4200);
            int num3 = (int)(400f * num2);
            for (int XCoord = 5; XCoord < Main.maxTilesX - 5; XCoord++)
            {
                int YCoord = 5;
                while ((double)YCoord < Main.worldSurface)
                {
                    if (Main.tile[XCoord, YCoord].active() && Main.tile[XCoord, YCoord].type == JoJoStands.Instance.TileType("ViralMeteoriteTile"))
                    {
                        num++;
                        if (num > num3)
                        {
                            return;
                        }
                    }
                    YCoord++;
                }
            }
            float lowestTCoordinate = 600f;
            while (!droppedMeteorite)
            {
                float spawnAreaDistance = (float)Main.maxTilesX * 0.08f;        //Section of the world that's considered "spawn area"
                int XCoord = Main.rand.Next(150, Main.maxTilesX - 150);
                while ((float)XCoord > (float)Main.spawnTileX - spawnAreaDistance && (float)XCoord < (float)Main.spawnTileX + spawnAreaDistance)        //When the chosen coordinate isn't near spawn
                {
                    XCoord = Main.rand.Next(150, Main.maxTilesX - 150);
                }
                int YCoord = (int)(Main.worldSurface * 0.3);
                while (YCoord < Main.maxTilesY)
                {
                    if (Main.tile[XCoord, YCoord].active() && Main.tileSolid[(int)Main.tile[XCoord, YCoord].type])
                    {
                        int YLevel = 0;
                        int meteoriteArea = 15;        //Size of the meteor as a radius
                        for (int i = XCoord - meteoriteArea; i < XCoord + meteoriteArea; i++)
                        {
                            for (int j = YCoord - meteoriteArea; j < YCoord + meteoriteArea; j++)
                            {
                                if (WorldGen.SolidTile(i, j))
                                {
                                    YLevel++;
                                    if (Main.tile[i, j].type == TileID.Cloud || Main.tile[i, j].type == TileID.Sunplate)
                                    {
                                        YLevel -= 100;
                                    }
                                }
                                else if (Main.tile[i, j].liquid > 0)
                                {
                                    YLevel--;
                                }
                            }
                        }
                        if ((float)YLevel < lowestTCoordinate)
                        {
                            lowestTCoordinate -= 0.5f;
                            break;
                        }
                        droppedMeteorite = GenereateViralMeteorite(XCoord, YCoord);
                        if (droppedMeteorite)
                        {
                            break;
                        }
                        break;
                    }
                    else
                    {
                        YCoord++;
                    }
                }
                if (lowestTCoordinate < 100f)
                {
                    return;
                }
            }
        }

        public static bool GenereateViralMeteorite(int i, int j)     //from WorldGen.cs, line 2303
        {
            int worldBoundaries = 50;
            if (i < worldBoundaries || i > Main.maxTilesX - worldBoundaries)
            {
                return false;
            }
            if (j < worldBoundaries || j > Main.maxTilesY - worldBoundaries)
            {
                return false;
            }
            int meteoriteArea = 35;         //The amount of space the VM will take up
            Rectangle meteoriteRect = new Rectangle((i - meteoriteArea) * 16, (j - meteoriteArea) * 16, meteoriteArea * 2 * 16, meteoriteArea * 2 * 16);
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];
                if (player.active)
                {
                    Rectangle playerArea = new Rectangle((int)(player.position.X + (float)(player.width / 2) - (float)(NPC.sWidth / 2) - (float)NPC.safeRangeX), (int)(player.position.Y + (float)(player.height / 2) - (float)(NPC.sHeight / 2) - (float)NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    if (meteoriteRect.Intersects(playerArea))
                    {
                        return false;
                    }
                }
            }
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    Rectangle npcArea = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);         //How much space that NPC is taking up and where
                    if (meteoriteRect.Intersects(npcArea))
                    {
                        return false;
                    }
                }
            }
            for (int xCoord = i - meteoriteArea; xCoord < i + meteoriteArea; xCoord++)
            {
                for (int yCoord = j - meteoriteArea; yCoord < j + meteoriteArea; yCoord++)
                {
                    if (Main.tile[xCoord, yCoord].active() && TileID.Sets.BasicChest[(int)Main.tile[xCoord, yCoord].type])
                    {
                        return false;
                    }
                }
            }
            meteoriteArea = WorldGen.genRand.Next(17, 23);
            for (int xCoord = i - meteoriteArea; xCoord < i + meteoriteArea; xCoord++)
            {
                for (int yCoord = j - meteoriteArea; yCoord < j + meteoriteArea; yCoord++)
                {
                    if (yCoord > j + Main.rand.Next(-2, 3) - 5)     //Deciding random Y placement
                    {
                        float tilePlacementX = (float)Math.Abs(i - xCoord);
                        float tilePlacementY = (float)Math.Abs(j - yCoord);
                        float num6 = (float)Math.Sqrt((double)(tilePlacementX * tilePlacementX + tilePlacementY * tilePlacementY));
                        if ((double)num6 < (double)meteoriteArea * 0.9 + (double)Main.rand.Next(-4, 5))
                        {
                            if (!Main.tileSolid[(int)Main.tile[xCoord, yCoord].type])
                            {
                                Main.tile[xCoord, yCoord].active(false);
                            }
                            Main.tile[xCoord, yCoord].type = (ushort)JoJoStands.Instance.TileType("ViralMeteoriteTile");
                        }
                    }
                }
            }
            meteoriteArea = WorldGen.genRand.Next(8, 14);
            for (int xCoord = i - meteoriteArea; xCoord < i + meteoriteArea; xCoord++)
            {
                for (int yCoord = j - meteoriteArea; yCoord < j + meteoriteArea; yCoord++)
                {
                    if (yCoord > j + Main.rand.Next(-2, 3) - 4)
                    {
                        float num9 = (float)Math.Abs(i - xCoord);
                        float num10 = (float)Math.Abs(j - yCoord);
                        float num11 = (float)Math.Sqrt((double)(num9 * num9 + num10 * num10));
                        if ((double)num11 < (double)meteoriteArea * 0.8 + (double)Main.rand.Next(-3, 4))
                        {
                            Main.tile[xCoord, yCoord].active(false);
                        }
                    }
                }
            }
            meteoriteArea = WorldGen.genRand.Next(25, 35);
            for (int xCoord = i - meteoriteArea; xCoord < i + meteoriteArea; xCoord++)
            {
                for (int yCoord = j - meteoriteArea; yCoord < j + meteoriteArea; yCoord++)
                {
                    float tilePlacementX = (float)Math.Abs(i - xCoord);
                    float tilePlacementY = (float)Math.Abs(j - yCoord);
                    float num16 = (float)Math.Sqrt((double)(tilePlacementX * tilePlacementX + tilePlacementY * tilePlacementY));
                    if ((double)num16 < (double)meteoriteArea * 0.7)
                    {
                        if (Main.tile[xCoord, yCoord].type == TileID.Trees || Main.tile[xCoord, yCoord].type == TileID.CorruptThorns || Main.tile[xCoord, yCoord].type == TileID.CrimtaneThorns)
                        {
                            WorldGen.KillTile(xCoord, yCoord, false, false, false);
                        }
                        Main.tile[xCoord, yCoord].liquid = 0;
                    }
                    if (Main.tile[xCoord, yCoord].type == JoJoStands.Instance.TileType("ViralMeteoriteTile"))
                    {
                        if (!WorldGen.SolidTile(xCoord - 1, yCoord) && !WorldGen.SolidTile(xCoord + 1, yCoord) && !WorldGen.SolidTile(xCoord, yCoord - 1) && !WorldGen.SolidTile(xCoord, yCoord + 1))
                        {
                            Main.tile[xCoord, yCoord].active(false);
                        }
                        else if ((Main.tile[xCoord, yCoord].halfBrick() || Main.tile[xCoord - 1, yCoord].topSlope()) && !WorldGen.SolidTile(xCoord, yCoord + 1))
                        {
                            Main.tile[xCoord, yCoord].active(false);
                        }
                    }
                    WorldGen.SquareTileFrame(xCoord, yCoord, true);
                    WorldGen.SquareWallFrame(xCoord, yCoord, true);
                }
            }
            meteoriteArea = WorldGen.genRand.Next(23, 32);
            for (int xCoord = i - meteoriteArea; xCoord < i + meteoriteArea; xCoord++)
            {
                for (int yCoord = j - meteoriteArea; yCoord < j + meteoriteArea; yCoord++)
                {
                    if (yCoord > j + WorldGen.genRand.Next(-3, 4) - 3 && Main.tile[xCoord, yCoord].active() && Main.rand.Next(10) == 0)
                    {
                        float tilePlacementX = (float)Math.Abs(i - xCoord);
                        float tilePlacementY = (float)Math.Abs(j - yCoord);
                        float num21 = (float)Math.Sqrt((double)(tilePlacementX * tilePlacementX + tilePlacementY * tilePlacementY));
                        if ((double)num21 < (double)meteoriteArea * 0.8)
                        {
                            if (Main.tile[xCoord, yCoord].type == TileID.Trees || Main.tile[xCoord, yCoord].type == TileID.CorruptThorns || Main.tile[xCoord, yCoord].type == TileID.CrimtaneThorns)
                            {
                                WorldGen.KillTile(xCoord, yCoord, false, false, false);
                            }
                            Main.tile[xCoord, yCoord].type = (ushort)JoJoStands.Instance.TileType("ViralMeteoriteTile");
                            WorldGen.SquareTileFrame(xCoord, yCoord, true);
                        }
                    }
                }
            }
            meteoriteArea = WorldGen.genRand.Next(30, 38);
            for (int xCoord = i - meteoriteArea; xCoord < i + meteoriteArea; xCoord++)
            {
                for (int yCoord = j - meteoriteArea; yCoord < j + meteoriteArea; yCoord++)
                {
                    if (yCoord > j + WorldGen.genRand.Next(-2, 3) && Main.tile[xCoord, yCoord].active() && Main.rand.Next(20) == 0)
                    {
                        float tilePlacementX = (float)Math.Abs(i - xCoord);
                        float tilePlacementY = (float)Math.Abs(j - yCoord);
                        float num26 = (float)Math.Sqrt((double)(tilePlacementX * tilePlacementX + tilePlacementY * tilePlacementY));
                        if ((double)num26 < (double)meteoriteArea * 0.85)
                        {
                            if (Main.tile[xCoord, yCoord].type == TileID.Trees || Main.tile[xCoord, yCoord].type == TileID.CorruptThorns || Main.tile[xCoord, yCoord].type == TileID.CrimtaneThorns)
                            {
                                WorldGen.KillTile(xCoord, yCoord, false, false, false);
                            }
                            Main.tile[xCoord, yCoord].type = (ushort)JoJoStands.Instance.TileType("ViralMeteoriteTile");
                            WorldGen.SquareTileFrame(xCoord, yCoord, true);
                        }
                    }
                }
            }
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText("A dangerous virus now inhabits " + Main.worldName + "...", 50, 255, 130, false);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromKey("A dangerous virus now inhabits " + Main.worldName + "...", new object[0]), new Color(50, 255, 130), -1);
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(-1, i, j, 40, TileChangeType.None);
            }
            return true;
        }
    }
}