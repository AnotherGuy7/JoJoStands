using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class AvdolHair : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortuneteller Wig");
            Tooltip.SetDefault("A wig, styled after a fortuneteller, and Stand user.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 6;
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