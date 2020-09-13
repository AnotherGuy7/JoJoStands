using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GratefulDeadT2 : StandItemClass
    {
        public override int standSpeed => 12;
        public override int standType => 1;

        public override string Texture
        {
            get { return mod.Name + "/Items/GratefulDeadT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grateful Dead (Tier 2)");
            Tooltip.SetDefault("Punch enemies to make them age and right-click to grab them!\nMore effective on hot biomes.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 41;
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
            recipe.AddIngredient(mod.ItemType("GratefulDeadT1"));
            recipe.AddIngredient(ItemID.Bone, 15);
            recipe.AddIngredient(mod.ItemType("WillToChange"));
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
