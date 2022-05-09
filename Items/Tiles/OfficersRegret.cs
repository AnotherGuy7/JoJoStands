using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Items.Tiles
{
    public class OfficersRegret : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Officer's Regret");
            Tooltip.SetDefault("`D. Storm`");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 1;
            Item.createTile = ModContent.TileType<OfficersRegretTile");
        }
    }
}