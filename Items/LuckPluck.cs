using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class LuckPluck : ModItem
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Luck and Pluck");
            // Tooltip.SetDefault("The sword of an old gladiator.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.width = 64;
            Item.height = 30;
            Item.knockBack = 5f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("JoJoStandsIron-TierBar", 6)
                .AddRecipeGroup("JoJoStandsGold-TierBar", 4)
                .AddIngredient(ItemID.Ruby)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
