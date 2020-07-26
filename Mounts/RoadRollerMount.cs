using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Mounts
{
	public class RoadRollerMount : ModMountData
	{
		public override void SetDefaults()			//basically the unicorn mount
		{
			mountData.buff = mod.BuffType("RoadRollerBuff");
			mountData.heightBoost = 0;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.2f;
			mountData.runSpeed = 5f;
			mountData.dashSpeed = 14f;
			mountData.acceleration = 0.5f;
			mountData.jumpHeight = 6;
			mountData.jumpSpeed = 6f;
			mountData.totalFrames = 2;
			mountData.constantJump = false;
			int[] array = new int[mountData.totalFrames];
			for (int num6 = 0; num6 < array.Length; num6++)
			{
				array[num6] = 28;
			}
			array[1] += 2;
			mountData.playerYOffsets = array;
			mountData.xOffset = 5;
			mountData.bodyFrame = 3;
			mountData.yOffset = 1;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 17;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 1;
			mountData.runningFrameDelay = 11;
			mountData.runningFrameStart = 0;
			mountData.dashingFrameCount = 0;
			mountData.dashingFrameDelay = 0;
			mountData.dashingFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 11;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 1;
			mountData.idleFrameDelay = 17;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode == NetmodeID.Server)
			{
				return;
			}

			mountData.textureWidth = mountData.backTexture.Width + 20;
			mountData.textureHeight = mountData.backTexture.Height;
		}

		public override void UpdateEffects(Player player)
		{
			Tile tilesToDestroy = Main.tile[(int)(player.Center.X + (mountData.textureWidth / 2) + 1) / 16, (int)player.Center.Y / 16];
			if (tilesToDestroy.active() && tilesToDestroy.type != 0)
			{
				tilesToDestroy.ClearTile();
			}
		}
	}
}