using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KingCrimsonT3 : StandItemClass
    {
        public override int standSpeed => 22;
        public override int standType => 1;
        public override string standProjectileName => "KingCrimson";
        public override int standTier => 3;

        public override string Texture
        {
            get { return mod.Name + "/Items/KingCrimsonT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("King Crimson (Tier 3)");
            Tooltip.SetDefault("Donut enemies with a powerful punch and hold right-click to block off enemies and reposition!\nConsecutive Donuts deal greater damage.\nSpecial: Skip 5 seconds of time!\nSecond Special: Use Epitaph for 4 seconds!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 124;
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
            recipe.AddIngredient(mod.ItemType("KingCrimsonT2"));
            recipe.AddIngredient(ItemID.SoulofFright, 4);
            recipe.AddIngredient(ItemID.SoulofSight, 6);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(mod.ItemType("WillToControl"), 2);
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
