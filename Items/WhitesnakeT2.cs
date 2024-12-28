using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class WhitesnakeT2 : StandItemClass
    {
        public override int StandSpeed => 13;
        public override int StandType => 2;
        public override string StandIdentifierName => "Whitesnake";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => WhitesnakeFinal.WhitesnakeTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/WhitesnakeT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Whitesnake (Tier 2)");
            /* Tooltip.SetDefault("Punch enemies at a really fast rate and right click to throw some acid!\nSecond Special: Remote Control" +
                "\nWhile in Remote Mode: Left-click to move and right-click shoot enemies with a pistol!\nUsed in Stand Slot"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 38;
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
                .AddIngredient(ModContent.ItemType<WhitesnakeT1>())
                .AddRecipeGroup("JoJoStandsEvilBar", 9)
                .AddIngredient(ItemID.Diamond)
                .AddRecipeGroup("JoJoStandsRottenVertebrae", 5)
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
