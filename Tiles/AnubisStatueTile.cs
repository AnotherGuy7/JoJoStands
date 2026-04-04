using JoJoStands;
using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Items.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace JoJoStands.Tiles
{
    public class AnubisStatueTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = false;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.newTile.Origin = new Point16(0, 2);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(80, 60, 120), CreateMapEntryName());

            DustType = DustID.Stone;
            RegisterItemDrop(ModContent.ItemType<AnubisStatue>());
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.25f;
            g = 0.18f;
            b = 0.02f;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX != 0 || tile.TileFrameY != 0)
                return;

            Vector2 tileCenter = new Vector2(i + 1, j + 1.5f) * 16f;
            float radius = 50 * 16f;

            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];
                if (!player.active || player.dead)
                    continue;
                if (Vector2.DistanceSquared(player.Center, tileCenter) <= radius * radius)
                    player.AddBuff(ModContent.BuffType<AnubisJudgment>(), 180);
            }
        }
    }
}