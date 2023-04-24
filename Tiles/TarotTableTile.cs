using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace JoJoStands.Tiles
{
    public class TarotTableTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            DustType = DustID.Lead;
            AnimationFrameHeight = 54;      //you put the entre animation frame size here, not in tiles
            TileID.Sets.DisableSmartCursor[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                18
            };
            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Tarot Table");
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

        public override bool RightClick(int i, int j)
        {
            Player player = Main.player[Main.myPlayer];
            player.AddBuff(ModContent.BuffType<StrongWill>(), 300 * 60);
            return true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 4, 4, ModContent.ItemType<TarotTable>());
        }
    }
}