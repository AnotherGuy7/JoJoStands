using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GratefulDeadT4 : StandItemClass
    {
        public override int standSpeed => 10;
        public override int standType => 1;

        public override string Texture
        {
            get { return mod.Name + "/Items/GratefulDeadT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grateful Dead (Final)");
            Tooltip.SetDefault("Punch enemies to make them age and right-click to grab them!\nSpecial: Spread Gas\nMore effective on hot biomes.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 90;
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
            recipe.AddIngredient(mod.ItemType("GratefulDeadT3"));
            recipe.AddIngredient(ItemID.ShroomiteBar, 14);
            recipe.AddIngredient(ItemID.Ichor, 20);
            recipe.AddIngredient(mod.ItemType("DeterminedLifeForce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GratefulDeadT3"));
            recipe.AddIngredient(ItemID.ShroomiteBar, 14);
            recipe.AddIngredient(ItemID.CursedFlame, 20);
            recipe.AddIngredient(mod.ItemType("DeterminedLifeForce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
