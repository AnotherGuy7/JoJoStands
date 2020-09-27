using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Tiles
{
    public class ViralBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A gold brick. Though it's alive!");
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
            item.rare = ItemRarityID.Blue;
            item.maxStack = 999;
            item.createTile = mod.TileType("ViralBrickTile");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 50);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"));
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
}