using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace JoJoStands.Tiles
{
    public class TarotTableTile : ModTile
    {
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			dustType = DustID.Lead;
			animationFrameHeight = 54;		//you put the entre animation frame size here, not in tiles
			disableSmartCursor = true;

			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Width = 2;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				18
			};
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Tarot Table");
			AddMapEntry(Color.Brown, name);
		}


		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter >= 12)
			{
				frame++;
				frameCounter = 0;
				if (frame >= 13)
				{
					frame = 0;
				}
			}
		}

		public override bool NewRightClick(int i, int j)
		{
			Player player = Main.player[Main.myPlayer];
			player.AddBuff(mod.BuffType("StrongWill"), 300 * 60);
			return true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 4, 4, mod.ItemType("TarotTable"));
		}
	}
}