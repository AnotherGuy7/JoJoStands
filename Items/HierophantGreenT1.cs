using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HierophantGreenT1 : StandItemClass
    {
        public override int standSpeed => 40;
        public override int standType => 2;
        public override string standProjectileName => "HierophantGreen";
        public override int standTier => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hierophant Green (Tier 1)");
            Tooltip.SetDefault("Shoot emeralds at the enemies!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 12;
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StandArrow"));
            recipe.AddIngredient(mod.ItemType("WillToProtect"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
