using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KingCrimsonFinal : StandItemClass
    {
        public override int standSpeed => 20;
        public override int standType => 1;
        public override string standProjectileName => "KingCrimson";
        public override int standTier => 4;

        public override string Texture
        {
            get { return mod.Name + "/Items/KingCrimsonT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("King Crimson (Final Tier)");
            Tooltip.SetDefault("Donut enemies with a powerful punch and hold right-click to block off enemies and reposition!\nConsecutive Donuts deal greater damage.\nSpecial: Skip 10 seconds of time!\nSecond Special: Use Epitaph for 9 seconds!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 186;
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
            recipe.AddIngredient(mod.ItemType("KingCrimsonT3"));
            recipe.AddIngredient(ItemID.ChlorophyteBar, 13);
            recipe.AddIngredient(ItemID.GoldCrown);
            recipe.AddIngredient(mod.ItemType("SoulofTime"), 2);
            recipe.AddIngredient(mod.ItemType("WillToControl"), 3);
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 3);
            recipe.AddIngredient(mod.ItemType("TaintedLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
