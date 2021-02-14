using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Mounts
{
	public class LeafGliderMount : ModMountData
	{
		public override void SetDefaults()
		{
			//mountData.buff = mod.BuffType("RoadRollerBuff");
			mountData.spawnDust = 3;
			mountData.heightBoost = 0;
			mountData.flightTimeMax = 5 * 60;
			mountData.fallDamage = 0f;
			mountData.runSpeed = 9f;
			mountData.dashSpeed = 0f;
			mountData.acceleration = 0.03f;
			mountData.jumpHeight = 0;
			mountData.jumpSpeed = 0;
			mountData.totalFrames = 4;
			mountData.constantJump = false;
			int[] totalFrames = new int[mountData.totalFrames];
			for (int frame = 0; frame < totalFrames.Length; frame++)
			{
				totalFrames[frame] = -6;
			}
			mountData.playerYOffsets = totalFrames;
			mountData.xOffset = -2;
			mountData.bodyFrame = 5;
			mountData.yOffset = -16;
			mountData.playerHeadOffset = 0;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 10;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 1;
			mountData.runningFrameDelay = 30;
			mountData.runningFrameStart = 0;
			mountData.dashingFrameCount = 1;
			mountData.dashingFrameDelay = 10;
			mountData.dashingFrameStart = 0;
			mountData.flyingFrameCount = 1;
			mountData.flyingFrameDelay = 10;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 10;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 1;
			mountData.idleFrameDelay = 10;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			mountData.textureWidth = mountData.backTexture.Width;
			mountData.textureHeight = mountData.backTexture.Height;
		}

		private int usageTimer = 0;
		private int SlowGlideFrames = 2;

		public override void UpdateEffects(Player player)
		{
			HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
			hPlayer.passiveRegen = false;
			if (player.whoAmI == Main.myPlayer && hPlayer.amountOfHamon > 5)
            {
				usageTimer++;
				if (usageTimer > 90)
                {
					hPlayer.amountOfHamon -= 1;
					usageTimer = 0;
                }
            }
			player.velocity.Y = 0.1f;
			player.fallStart = (int)player.position.Y;
			if (!player.controlUp || WorldGen.SolidTile((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f) + 2) || hPlayer.amountOfHamon <= 5)
            {
				player.mount.Dismount(player);
            }
		}

		public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
		{
			Mount mount = mountedPlayer.mount;
			float positiveVelX = Math.Abs(velocity.X);
			bool glidingFast = positiveVelX > 7f;
			mount._frameCounter += 1f + positiveVelX / 2f;
			if (glidingFast)
			{
				if (mount._frameCounter >= mountData.runningFrameDelay)
				{
					mount._frame += 1;
					mount._frameCounter = 0;
					if (mount._frame >= mountData.totalFrames)
					{
						mount._frame = 0;
					}
				}
			}
			else
            {
				if (mount._frameCounter >= mountData.runningFrameDelay)
				{
					mount._frame += 1;
					mount._frameCounter = 0;
					if (mount._frame >= SlowGlideFrames)
                    {
						mount._frame = 0;
                    }
				}
			}
			return false;
		}

		public override void Dismount(Player player, ref bool skipDust)
        {
			usageTimer = 0;
        }
	}
}