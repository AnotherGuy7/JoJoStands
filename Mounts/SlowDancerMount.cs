using JoJoStands.Buffs.ItemBuff;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Mounts
{
    public class SlowDancerMount : ModMount
    {
        public override void SetStaticDefaults()          //basically the unicorn mount
        {
            MountData.buff = ModContent.BuffType<SlowDancerBuff>();
            MountData.heightBoost = 34;
            MountData.flightTimeMax = 0;
            MountData.fallDamage = 0.2f;
            MountData.dashSpeed = 14f;
            MountData.runSpeed = 7f;
            MountData.acceleration = 0.5f;
            MountData.jumpHeight = 6;
            MountData.jumpSpeed = 6f;
            MountData.totalFrames = 16;
            MountData.constantJump = true;
            int[] array = new int[MountData.totalFrames];
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
            MountData.playerYOffsets = array;
            MountData.xOffset = 5;
            MountData.bodyFrame = 3;
            MountData.yOffset = 1;
            MountData.playerHeadOffset = 38;
            MountData.idleFrameCount = 0;       //Idle
            MountData.idleFrameDelay = 0;
            MountData.idleFrameStart = 0;
            MountData.standingFrameCount = 1;       //Standing
            MountData.standingFrameDelay = 12;
            MountData.standingFrameStart = 0;
            MountData.runningFrameCount = 7;        //Running
            MountData.runningFrameDelay = 15;
            MountData.runningFrameStart = 1;
            MountData.dashingFrameCount = 7;        //Dashing (When does this even happen?)
            MountData.dashingFrameDelay = 20;
            MountData.dashingFrameStart = 9;
            MountData.flyingFrameCount = 6;     //Flying
            MountData.flyingFrameDelay = 6;
            MountData.flyingFrameStart = 1;
            MountData.inAirFrameCount = 1;      //In Air
            MountData.inAirFrameDelay = 12;
            MountData.inAirFrameStart = 15;
            MountData.idleFrameLoop = false;
            MountData.swimFrameCount = MountData.inAirFrameCount;       //Swimming
            MountData.swimFrameDelay = MountData.inAirFrameDelay;
            MountData.swimFrameStart = MountData.inAirFrameStart;
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            MountData.textureWidth = MountData.backTexture.Width() + 20;
            MountData.textureHeight = MountData.backTexture.Height();
        }

        public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
        {
            Mount mount = mountedPlayer.mount;
            float positiveVelX = Math.Abs(velocity.X);
            bool dashing = positiveVelX > MountData.dashSpeed - MountData.runSpeed / 2f;
            if (velocity == Vector2.Zero)
            {
                mount._frame = 0;
            }
            else
            {
                mount._frameCounter += 1 + positiveVelX / 2f;
                if (dashing)
                {
                    if (mount._frameCounter >= MountData.dashingFrameDelay)
                    {
                        mount._frame += 1;
                        mount._frameCounter = 0;
                    }
                    if (mount._frame < MountData.dashingFrameStart)
                    {
                        mount._frame = MountData.dashingFrameStart;
                    }
                    if (mount._frame >= MountData.dashingFrameStart + MountData.dashingFrameCount - 1)
                    {
                        mount._frame = MountData.dashingFrameStart;
                    }
                }
                else
                {
                    if (mount._frameCounter >= MountData.runningFrameDelay)
                    {
                        mount._frame += 1;
                        mount._frameCounter = 0;
                    }
                    if (mount._frame < MountData.runningFrameStart)
                    {
                        mount._frame = MountData.runningFrameStart;
                    }
                    if (mount._frame >= MountData.runningFrameStart + MountData.runningFrameCount)
                    {
                        mount._frame = MountData.runningFrameStart;
                    }
                }
            }
            if (!WorldGen.SolidTile((int)(mountedPlayer.position.X / 16f), (int)(mountedPlayer.position.Y / 16f) + 5) || mountedPlayer.velocity.Y != 0f)
                mount._frame = 15;

            return false;
        }

        private readonly Vector2 size = new Vector2(126, 82);

        public override void UpdateEffects(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Math.Abs(player.velocity.X) >= 13f)
            {
                mPlayer.slowDancerSprintTime++;
                mPlayer.goldenSpinCounter += 1;
                if (mPlayer.slowDancerSprintTime >= 60)
                    mPlayer.goldenSpinCounter += mPlayer.slowDancerSprintTime / 60;
            }
            else
                mPlayer.slowDancerSprintTime = 0;

            if (mPlayer.slowDancerSprintTime >= 60)
            {
                for (int i = 0; i < Main.rand.Next(4, 6 + 1); i++)
                {
                    Vector2 dustSpeed = player.velocity * (Main.rand.Next(8, 10) / 10f);
                    Dust.NewDust(player.MountedCenter - (size / 2f), (int)size.X, (int)size.Y, DustID.IchorTorch, dustSpeed.X, dustSpeed.Y);
                }
            }
        }
    }
}