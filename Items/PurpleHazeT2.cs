using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class PurpleHazeT2 : StandItemClass
    {
        public override string Texture => Mod.Name + "/Items/PurpleHazeT1";
        public override int StandSpeed => 15;
        public override int StandType => 1;
        public override string StandIdentifierName => "PurpleHaze";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => PurpleHazeFinal.PurpleHazeTierColor;


        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.damage = 29;
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
                .AddIngredient(ModContent.ItemType<PurpleHazeT1>())
                .AddIngredient(ItemID.JungleSpores, 3)
                .AddIngredient(ItemID.Stinger, 6)
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddIngredient(ModContent.ItemType<WillToDestroy>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
