using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            item.width = 32;
            item.height = 32;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 1;
            item.consumable = true;
            item.value = Item.buyPrice(0, 20, 50, 0);
            item.rare = ItemRarityID.Blue;
            item.maxStack = 1;
            item.createTile = mod.TileType("TarotTableTile");
        }
    }
}