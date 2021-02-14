using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class AerosmithT2 : StandItemClass
    {
        public override int standSpeed => 10;
        public override int standType => 2;

        public override string Texture
        {
            get { return mod.Name + "/Items/AerosmithT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aerosmith (Tier 2)");
            Tooltip.SetDefault("Left-click to move and right-click to shoot bullets at the enemies!\nSpecial: Drop a bomb on enemies!\nThe farther the stand is from you, the less damage it does.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 42;
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
            recipe.AddIngredient(mod.ItemType("AerosmithT1"));
            recipe.AddIngredient(ItemID.Hellstone, 18);
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddIngredient(mod.ItemType("WillToProtect"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
