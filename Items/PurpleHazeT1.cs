using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class PurpleHazeT1 : StandItemClass
    {
        public override int StandSpeed => 16;
        public override int StandType => 1;
        public override string StandIdentifierName => "PurpleHaze";
        public override int StandTier => 1;
        public override Color StandTierDisplayColor => PurpleHazeFinal.PurpleHazeTierColor;


        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.damage = 9;
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
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
