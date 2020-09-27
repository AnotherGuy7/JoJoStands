using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Tiles
{
    public class ViralToilet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A toilet in which the toilet itself is a living creature.");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 10;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = Item.buyPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Yellow;
            item.maxStack = 999;
            item.createTile = mod.TileType("ViralToiletTile");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 6);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}