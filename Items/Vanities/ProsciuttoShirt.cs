using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Body)]
    public class ProsciuttoShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brotherly Suit");
            // Tooltip.SetDefault("A navy suit with white stripes.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.rare = ItemRarityID.LightPurple;
            Item.vanity = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 10)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}