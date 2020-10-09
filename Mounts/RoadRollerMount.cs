using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Mounts
{
	public class RoadRollerMount : ModMountData
	{
		public override void SetDefaults()
		{
			mountData.buff = mod.BuffType("RoadRollerBuff");
			mountData.heightBoost = 0;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 1f;
			mountData.runSpeed = 2f;
			mountData.dashSpeed = 3f;
			mountData.acceleration = 0.03f;
			mountData.jumpHeight = 3;
			mountData.jumpSpeed = 3f;
			mountData.totalFrames = 4;
			mountData.constantJump = false;
			int[] totalFrames = new int[mountData.totalFrames];
			for (int frame = 0; frame < totalFrames.Length; frame++)
			{
				totalFrames[frame] = 44;
			}
			mountData.playerYOffsets = totalFrames;
			mountData.xOffset = -15;
			mountData.bodyFrame = 3;
			mountData.yOffset = -16;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 17;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 10;
			mountData.runningFrameStart = 0;
			mountData.dashingFrameCount = 4;
			mountData.dashingFrameDelay = 10;
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

			mountData.textureWidth = mountData.backTexture.Width;
			mountData.textureHeight = mountData.backTexture.Height;
		}

		public override void UpdateEffects(Player player)
		{
			int Xdetection = (int)(player.position.X + (mountData.textureWidth / 2 * player.direction) + 1) / 16;
			int Ydetection = (int)(player.position.Y / 16f) + 1;
			Tile tileToDestroy1 = Main.tile[Xdetection, Ydetection];
			if (tileToDestroy1.active())
			{
				WorldGen.KillTile(Xdetection, Ydetection);
				NetMessage.SendTileSquare(-1, Xdetection, Ydetection, 1);
			}
			Tile tileToDestroy2 = Main.tile[Xdetection, Ydetection + 1];
			if (tileToDestroy2.active())
			{
				WorldGen.KillTile(Xdetection, Ydetection + 1);
				NetMessage.SendTileSquare(-1, Xdetection, Ydetection + 1, 1);
			}
			Tile tileToDestroy3 = Main.tile[Xdetection, Ydetection - 1];
			if (tileToDestroy3.active())
			{
				WorldGen.KillTile(Xdetection, Ydetection - 1);
				NetMessage.SendTileSquare(-1, Xdetection, Ydetection - 1, 1);
			}
		}
	}
}