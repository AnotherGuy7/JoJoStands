using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
    public class ChlorositeBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorosite Bar");
            Tooltip.SetDefault("A gem of nature corrupted by an otherworldly virus...");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 1, 90, 0);
            Item.rare = 1;
            Item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemID.ChlorophyteBar)
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>())
                .AddTile(TileID.AdamantiteForge)
                .Register();
        }
    }
}