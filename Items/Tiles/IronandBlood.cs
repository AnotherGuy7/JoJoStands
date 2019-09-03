using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Tiles
{
    public class IronandBlood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iron and Blood");
            Tooltip.SetDefault("`D. Storm`");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 1;
            item.consumable = true;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 0, 50, 0);
            item.rare = 1;
            item.createTile = mod.TileType("IronandBloodTile");
        }
    }
}