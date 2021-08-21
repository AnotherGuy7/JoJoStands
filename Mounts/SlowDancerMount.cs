using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Mounts
{
    public class SlowDancerMount : ModMountData
    {
        public override void SetDefaults()          //basically the unicorn mount
        {
            mountData.buff = mod.BuffType("SlowDancerBuff");
            mountData.heightBoost = 34;
            mountData.flightTimeMax = 0;
            mountData.fallDamage = 0.2f;
            mountData.dashSpeed = 14f;
            mountData.runSpeed = 7f;
            mountData.acceleration = 0.5f;
            mountData.jumpHeight = 6;
            mountData.jumpSpeed = 6f;
            mountData.totalFrames = 16;
            mountData.constantJump = true;
            int[] array = new int[mountData.totalFrames];
            for (int num6 = 0; num6 < array.Length; num6++)
            {
                array[num6] = 28;
            }
            array[3] += 2;
            array[4] += 2;
            array[7] += 2;
            array[8] += 2;
            array[12] += 2;
            array[13] += 2;
            array[15] += 4;
            mountData.playerYOffsets = array;
            mountData.xOffset = 5;
            mountData.bodyFrame = 3;
            mountData.yOffset = 1;
            mountData.playerHeadOffset = 38;
            mountData.idleFrameCount = 0;       //Idle
            mountData.idleFrameDelay = 0;
            mountData.idleFrameStart = 0;
            mountData.standingFrameCount = 1;       //Standing
            mountData.standingFrameDelay = 12;
            mountData.standingFrameStart = 0;
            mountData.runningFrameCount = 7;        //Running
            mountData.runningFrameDelay = 15;
            mountData.runningFrameStart = 1;
            mountData.dashingFrameCount = 7;        //Dashing (When does this even happen?)
            mountData.dashingFrameDelay = 20;
            mountData.dashingFrameStart = 9;
            mountData.flyingFrameCount = 6;     //Flying
            mountData.flyingFrameDelay = 6;
            mountData.flyingFrameStart = 1;
            mountData.inAirFrameCount = 1;      //In Air
            mountData.inAirFrameDelay = 12;
            mountData.inAirFrameStart = 15;
            mountData.idleFrameLoop = false;
            mountData.swimFrameCount = mountData.inAirFrameCount;       //Swimming
            mountData.swimFrameDelay = mountData.inAirFrameDelay;
            mountData.swimFrameStart = mountData.inAirFrameStart;
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            mountData.textureWidth = mountData.backTexture.Width + 20;
            mountData.textureHeight = mountData.backTexture.Height;
        }

        public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
        {
            Mount mount = mountedPlayer.mount;
            float positiveVelX = Math.Abs(velocity.X);
            bool dashing = positiveVelX > mountData.dashSpeed - mountData.runSpeed / 2f;
            if (velocity == Vector2.Zero)
            {
                mount._frame = 0;
            }
            else
            {
                mount._frameCounter += 1 + positiveVelX / 2f;
                if (dashing)
                {
                    if (mount._frameCounter >= mountData.dashingFrameDelay)
                    {
                        mount._frame += 1;
                        mount._frameCounter = 0;
                    }
                    if (mount._frame < mountData.dashingFrameStart)
                    {
                        mount._frame = mountData.dashingFrameStart;
                    }
                    if (mount._frame >= mountData.dashingFrameStart + mountData.dashingFrameCount - 1)
                    {
                        mount._frame = mountData.dashingFrameStart;
                    }
                }
                else
                {
                    if (mount._frameCounter >= mountData.runningFrameDelay)
                    {
                        mount._frame += 1;
                        mount._frameCounter = 0;
                    }
                    if (mount._frame < mountData.runningFrameStart)
                    {
                        mount._frame = mountData.runningFrameStart;
                    }
                    if (mount._frame >= mountData.runningFrameStart + mountData.runningFrameCount)
                    {
                        mount._frame = mountData.runningFrameStart;
                    }
                }
            }
            if (!WorldGen.SolidTile((int)(mountedPlayer.position.X / 16f), (int)(mountedPlayer.position.Y / 16f) + 5) || mountedPlayer.velocity.Y != 0f)
            {
                mount._frame = 15;
            }
            return false;
        }

        public override void UpdateEffects(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Math.Abs(player.velocity.X) >= 13f)
            {
                mPlayer.goldenSpinCounter += 1;
            }
        }
    }
}