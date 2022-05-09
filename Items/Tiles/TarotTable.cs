using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Items.Tiles
{
    public class TarotTable : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tarot Table");
            Tooltip.SetDefault("A table that reacts to Stand Users, granting them greater power as they wish it.");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 20, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 1;
            Item.createTile = ModContent.TileType<TarotTableTile");
        }
    }
}