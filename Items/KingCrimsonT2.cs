using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KingCrimsonT2 : StandItemClass
    {
        public override int StandSpeed => 24;
        public override int StandType => 1;
        public override string StandIdentifierName => "KingCrimson";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => KingCrimsonFinal.KingCrimsonTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/KingCrimsonT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("King Crimson (Tier 2)");
            // Tooltip.SetDefault("Donut enemies with a powerful punch and hold right-click to block off enemies and reposition!\nSpecial: Skip 2 seconds of time!\nPassive: Consecutive Donuts deal greater damage.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 74;
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
                .AddIngredient(ModContent.ItemType<KingCrimsonT1>())
                .AddIngredient(ItemID.Hellstone, 15)
                .AddRecipeGroup("JoJoStandsEvilBar", 3)
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddIngredient(ModContent.ItemType<WillToEscape>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
