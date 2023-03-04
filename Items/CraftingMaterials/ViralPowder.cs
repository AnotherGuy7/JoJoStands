using JoJoStands.Items.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
    public class ViralPowder : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Viral Meteorite turned into dust. Tastes pretty good!");
            SacrificeTotal = 50;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 20;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(copper: 25);
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ModContent.ItemType<ViralMeteorite>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}