using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HierophantGreenT2 : StandItemClass
    {
        public override int standSpeed => 30;
        public override int standType => 2;

        public override string Texture
        {
            get { return mod.Name + "/Items/HierophantGreenT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hierophant Green (Tier 2)");
            Tooltip.SetDefault("Shoot emeralds at the enemies and right-click to set two points for an Emerald Tripwire!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 32;
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
            recipe.AddIngredient(mod.ItemType("HierophantGreenT1"));
            recipe.AddIngredient(ItemID.Emerald, 7);
            recipe.AddIngredient(mod.ItemType("WillToProtect"));
            recipe.AddIngredient(mod.ItemType("WillToChange"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
