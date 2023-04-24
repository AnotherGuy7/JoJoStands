using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Tiles
{
    public class TarotTable : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tarot Table");
            // Tooltip.SetDefault("A table that reacts to Stand Users, granting them greater power as they wish it.");
            Item.ResearchUnlockCount = 1;
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
            Item.value = Item.buyPrice(gold: 15, silver: 50);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 1;
            Item.createTile = ModContent.TileType<TarotTableTile>();
        }
    }
}