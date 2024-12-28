using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Tiles
{
    public class ViralBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A gold brick. Though it's alive!");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(silver: 30);
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 999;
            Item.createTile = ModContent.TileType<ViralBrickTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(50)
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddIngredient(ModContent.ItemType<ViralMeteorite>())
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}