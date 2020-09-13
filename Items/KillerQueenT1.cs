using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KillerQueenT1 : StandItemClass
    {
        public override int standSpeed => 12;
        public override int standType => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Killer Queen (1st Bomb)");
            Tooltip.SetDefault("Left-click to punch and right-click to trigger any block! \nRange: 10 blocks\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 14;
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
            recipe.AddIngredient(mod.ItemType("WillToDestroy"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}