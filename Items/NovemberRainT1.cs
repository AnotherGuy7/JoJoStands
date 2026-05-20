using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class NovemberRainT1 : StandItemClass
    {
        public override int StandSpeed => 30;
        public override int StandType => 2;
        public override string StandIdentifierName => "NovemberRain";
        public override int StandTier => 1;
        public override Color StandTierDisplayColor => Color.SkyBlue;

        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddIngredient(ItemID.Cloud, 15)
                .AddIngredient(ItemID.BottledWater, 5)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
