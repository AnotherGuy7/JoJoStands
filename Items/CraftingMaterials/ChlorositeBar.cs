using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
    public class ChlorositeBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Chlorosite Bar");
            // Tooltip.SetDefault("A gem of nature corrupted by an otherworldly virus...");
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Yellow;
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