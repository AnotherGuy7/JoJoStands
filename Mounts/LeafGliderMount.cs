using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Mounts
{
	public class LeafGliderMount : ModMount
	{
		public override void SetStaticDefaults()
		{
			//MountData.buff = ModContent.BuffType<RoadRollerBuff>();
			MountData.spawnDust = 3;
			MountData.heightBoost = 0;
			MountData.flightTimeMax = 5 * 60;
			MountData.fallDamage = 0f;
			MountData.runSpeed = 9f;
			MountData.dashSpeed = 0f;
			MountData.acceleration = 0.03f;
			MountData.jumpHeight = 0;
			MountData.jumpSpeed = 0;
			MountData.totalFrames = 4;
			MountData.constantJump = false;
			int[] totalFrames = new int[MountData.totalFrames];
			for (int frame = 0; frame < totalFrames.Length; frame++)
			{
				totalFrames[frame] = -6;
			}
			MountData.playerYOffsets = totalFrames;
			MountData.xOffset = -2;
			MountData.bodyFrame = 5;
			MountData.yOffset = -16;
			MountData.playerHeadOffset = 0;
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 10;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = 1;
			MountData.runningFrameDelay = 30;
			MountData.runningFrameStart = 0;
			MountData.dashingFrameCount = 1;
			MountData.dashingFrameDelay = 10;
			MountData.dashingFrameStart = 0;
			MountData.flyingFrameCount = 1;
			MountData.flyingFrameDelay = 10;
			MountData.flyingFrameStart = 0;
			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 10;
			MountData.inAirFrameStart = 0;
			MountData.idleFrameCount = 1;
			MountData.idleFrameDelay = 10;
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
				player.mount.Dismount(player);
		}

		public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
		{
			Mount mount = mountedPlayer.mount;
			float positiveVelX = Math.Abs(velocity.X);
			bool glidingFast = positiveVelX > 7f;
			mount._frameCounter += 1f + positiveVelX / 2f;
			if (glidingFast)
			{
				if (mount._frameCounter >= MountData.runningFrameDelay)
				{
					mount._frame += 1;
					mount._frameCounter = 0;
					if (mount._frame >= MountData.totalFrames)
					{
						mount._frame = 0;
					}
				}
			}
			else
            {
				if (mount._frameCounter >= MountData.runningFrameDelay)
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
			player.GetModPlayer<HamonPlayer>().leafGliderGenerationTimer = 0;
        }
	}
}