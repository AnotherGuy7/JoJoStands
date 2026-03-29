using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class PurpleHazeFinal : StandItemClass
    {
        public override string Texture => Mod.Name + "/Items/PurpleHazeT1";
        public override int StandSpeed => 13;
        public override int StandType => 1;
        public override string StandIdentifierName => "PurpleHaze";
        public override int StandTier => 4;
        public override Color StandTierDisplayColor => PurpleHazeTierColor;
        public static Color PurpleHazeTierColor => new Color(81, 44, 123);


        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crazy Diamond (Tier 1)");
            /* Tooltip.SetDefault(
                "Left-click to punch enemies at a really fast rate!" +
                "\nSpecial: Switch to Restoration Mode" +
                "\nLeft-click in Restoration Mode to perform a restorative barrage and right-click to restore your item to it's component state." +
                "\nUsed in Stand Slot"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 65;
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
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
