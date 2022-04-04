using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class DarkTopHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A top hat worn most during the 1800's.");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.rare = ItemRarityID.LightPurple;
            item.vanity = true;
        }

        public override bool DrawHead()
        {
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 2);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}