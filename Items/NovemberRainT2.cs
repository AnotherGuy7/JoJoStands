using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class NovemberRainT2 : StandItemClass
    {
        public override int StandSpeed => 6;
        public override int StandType => 1;
        public override string StandIdentifierName => "NovemberRain";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => Color.SkyBlue;

        public override string Texture => Mod.Name + "/Items/NovemberRainT1";

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<NovemberRainT1>())
                .AddRecipeGroup("JoJoStandsGold-TierBar", 12)
                .AddIngredient(ItemID.HellstoneBar, 20)
                .AddIngredient(ItemID.IceBlock, 25)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
