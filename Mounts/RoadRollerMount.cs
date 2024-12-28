using JoJoStands.Buffs.ItemBuff;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Mounts
{
    public class RoadRollerMount : ModMount
    {
        public override void SetStaticDefaults()
        {
            MountData.buff = ModContent.BuffType<RoadRollerBuff>();
            MountData.heightBoost = 0;
            MountData.flightTimeMax = 0;
            MountData.fallDamage = 1f;
            MountData.runSpeed = 2f;
            MountData.dashSpeed = 3f;
            MountData.acceleration = 0.03f;
            MountData.jumpHeight = 3;
            MountData.jumpSpeed = 3f;
            MountData.totalFrames = 4;
            MountData.constantJump = false;
            int[] totalFrames = new int[MountData.totalFrames];
            for (int frame = 0; frame < totalFrames.Length; frame++)
            {
                totalFrames[frame] = 44;
            }
            MountData.playerYOffsets = totalFrames;
            MountData.xOffset = -15;
            MountData.bodyFrame = 3;
            MountData.yOffset = -16;
            MountData.playerHeadOffset = 22;
            MountData.standingFrameCount = 1;
            MountData.standingFrameDelay = 17;
            MountData.standingFrameStart = 0;
            MountData.runningFrameCount = 4;
            MountData.runningFrameDelay = 10;
            MountData.runningFrameStart = 0;
            MountData.dashingFrameCount = 4;
            MountData.dashingFrameDelay = 10;
            MountData.dashingFrameStart = 0;
            MountData.flyingFrameCount = 0;
            MountData.flyingFrameDelay = 0;
            MountData.flyingFrameStart = 0;
            MountData.inAirFrameCount = 1;
            MountData.inAirFrameDelay = 11;
            MountData.inAirFrameStart = 0;
            MountData.idleFrameCount = 1;
            MountData.idleFrameDelay = 17;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = false;
            MountData.swimFrameCount = MountData.inAirFrameCount;
            MountData.swimFrameDelay = MountData.inAirFrameDelay;
            MountData.swimFrameStart = MountData.inAirFrameStart;
            if (Main.netMode == NetmodeID.Server)
                return;

            MountData.textureWidth = MountData.backTexture.Width();
            MountData.textureHeight = MountData.backTexture.Height();
        }

        public override void UpdateEffects(Player player)
        {
            int Xdetection = (int)(player.position.X + (MountData.textureWidth / 2 * player.direction) + 1) / 16;
            int Ydetection = (int)(player.position.Y / 16f) + 1;
            Tile tileToDestroy1 = Main.tile[Xdetection, Ydetection];
            if (tileToDestroy1.HasTile && tileToDestroy1.TileType != TileID.LihzahrdBrick && tileToDestroy1.TileType != TileID.Chlorophyte)
            {
                WorldGen.KillTile(Xdetection, Ydetection);
                NetMessage.SendTileSquare(-1, Xdetection, Ydetection, 1);
            }
            Tile tileToDestroy2 = Main.tile[Xdetection, Ydetection + 1];
            if (tileToDestroy2.HasTile && tileToDestroy2.TileType != TileID.LihzahrdBrick && tileToDestroy2.TileType != TileID.Chlorophyte)
            {
                WorldGen.KillTile(Xdetection, Ydetection + 1);
                NetMessage.SendTileSquare(-1, Xdetection, Ydetection + 1, 1);
            }
            Tile tileToDestroy3 = Main.tile[Xdetection, Ydetection - 1];
            if (tileToDestroy3.HasTile && tileToDestroy3.TileType != TileID.LihzahrdBrick && tileToDestroy3.TileType != TileID.Chlorophyte)
            {
                WorldGen.KillTile(Xdetection, Ydetection - 1);
                NetMessage.SendTileSquare(-1, Xdetection, Ydetection - 1, 1);
            }

            Rectangle roadRollerRect = new Rectangle((int)player.position.X - MountData.textureWidth / 2, (int)player.position.Y, MountData.textureWidth, MountData.textureHeight);
            float positiveVel = Math.Abs(player.velocity.X);
            if (positiveVel > 0)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        if (npc.lifeMax > 5 && !npc.townNPC && !npc.friendly && !npc.immortal && !npc.hide && roadRollerRect.Intersects(npc.Hitbox))
                        {
                            NPC.HitInfo hitInfo = new NPC.HitInfo()
                            {
                                Damage = 35 + (int)(positiveVel * 2f),
                                Knockback = 7f + positiveVel,
                                HitDirection = player.direction
                            };
                            npc.StrikeNPC(hitInfo);
                            player.velocity *= 0.5f;
                        }
                    }
                }
            }
        }
    }
}