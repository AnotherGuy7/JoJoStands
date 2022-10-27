using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
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
            get { return Mod.Name + "/Items/KingCrimsonT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("King Crimson (Final Tier)");
            Tooltip.SetDefault("Donut enemies with a powerful punch and hold right-click to block off enemies and reposition!\nSpecial: Skip 10 seconds of time!\nSecond Special: Use Epitaph for 9 seconds!\nPassive: Consecutive Donuts deal greater damage.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 186;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<KingCrimsonT3>())
                .AddIngredient(ItemID.ChlorophyteBar, 13)
                .AddRecipeGroup("JoJoStandsCrown")
                .AddIngredient(ModContent.ItemType<SoulofTime>(), 2)
                .AddIngredient(ModContent.ItemType<WillToControl>(), 3)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 3)
                .AddIngredient(ModContent.ItemType<TaintedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
