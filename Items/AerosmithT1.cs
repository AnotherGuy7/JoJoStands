using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class AerosmithT1 : StandItemClass
    {
        public override int standSpeed => 12;
        public override int standType => 2;
        public override string standProjectileName => "Aerosmith";
        public override int standTier => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aerosmith (Tier 1)");
            Tooltip.SetDefault("Left-click to move and right-click to shoot bullets at the enemies!\nThe farther the stand is from you, the less damage it does.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 10;
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
