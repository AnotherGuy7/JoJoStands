using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Body)]
    public class DarkCape : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A fancy cape worn during the 1800's.");
        }

        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 32;
            item.rare = ItemRarityID.LightPurple;
            item.vanity = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}