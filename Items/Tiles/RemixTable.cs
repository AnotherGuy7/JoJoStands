using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Tiles
{
    public class RemixTable : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Remix Table");
            Tooltip.SetDefault("A plain looking DJ’s table, imbued with the Meteoric virus. Allows you to create and modify Stands.");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 45, 0);
            Item.rare = 1;
            Item.maxStack = 999;
            Item.createTile = ModContent.TileType<RemixTableTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()      //anvil + copper + any will
                .AddIngredient(ItemID.IronBar, 5)
                .AddIngredient(ItemID.ShadowScale)
                .AddRecipeGroup("JoJoStandsWills")
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 5)
                .AddIngredient(ItemID.ShadowScale, 3)
                .AddRecipeGroup("JoJoStandsWills")
                .AddTile(TileID.Anvils);
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 5)
                .AddIngredient(ItemID.TissueSample, 3)
                .AddRecipeGroup("JoJoStandsWills")
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 5)
                .AddIngredient(ItemID.TissueSample, 3)
                .AddRecipeGroup("JoJoStandsWills")
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}