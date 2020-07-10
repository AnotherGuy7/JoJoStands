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
			animationFrameHeight = 66;		//you put the entre animation frame size here, not in tiles
			disableSmartCursor = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				18
			};
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Tarot Table");
			AddMapEntry(Color.Brown, name);
		}

		public override void MouseOver(int i, int j)
		{
			base.MouseOver(i, j);
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter >= 12)
			{
				frame++;
				frameCounter = 0;
				if (frame >= 14)
				{
					frame = 0;
				}
			}
		}

		public override bool NewRightClick(int i, int j)
		{
			Player player = Main.player[Main.myPlayer];
			player.AddBuff(mod.BuffType("Motivated"), 300 * 60);
			return true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 4, 4, mod.ItemType("TarotTable"));
		}
	}
}