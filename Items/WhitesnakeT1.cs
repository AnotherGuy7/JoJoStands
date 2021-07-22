using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class WhitesnakeT1 : StandItemClass
    {
        public override int standSpeed => 14;
        public override int standType => 2;
        public override string standProjectileName => "Whitesnake";
        public override int standTier => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whitesnake (Tier 1)");
            Tooltip.SetDefault("Punch enemies at a really fast rate!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 19;
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
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
