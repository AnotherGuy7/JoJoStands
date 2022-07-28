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
            SacrificeTotal = 1;
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
            Item.value = Item.buyPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 999;
            Item.createTile = ModContent.TileType<RemixTableTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()      //anvil + copper + any will
                .AddRecipeGroup("JoJoStandsIron-TierBar", 5)
                .AddRecipeGroup("JoJoStandsShadowTissue", 3)
                .AddRecipeGroup("JoJoStandsWills")
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}