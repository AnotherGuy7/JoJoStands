using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KingCrimsonT2 : StandItemClass
    {
        public override int standSpeed => 24;
        public override int standType => 1;
        public override string standProjectileName => "KingCrimson";
        public override int standTier => 2;

        public override string Texture
        {
            get { return mod.Name + "/Items/KingCrimsonT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("King Crimson (Tier 2)");
            Tooltip.SetDefault("Donut enemies with a powerful punch and hold right-click to block off enemies and reposition!\nConsecutive Donuts deal greater damage.\nSpecial: Skip 2 seconds of time!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 74;
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
            recipe.AddIngredient(mod.ItemType("KingCrimsonT1"));
            recipe.AddIngredient(ItemID.Hellstone, 15);
            recipe.AddIngredient(ItemID.CrimtaneBar, 3);
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddIngredient(mod.ItemType("WillToEscape"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("KingCrimsonT1"));
            recipe.AddIngredient(ItemID.Hellstone, 15);
            recipe.AddIngredient(ItemID.DemoniteBar, 3);
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddIngredient(mod.ItemType("WillToEscape"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
