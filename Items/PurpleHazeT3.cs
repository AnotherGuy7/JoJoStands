using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class PurpleHazeT3 : StandItemClass
    {
        public override string Texture => Mod.Name + "/Items/PurpleHazeT1";
        public override int StandSpeed => 13;
        public override int StandType => 1;
        public override string StandIdentifierName => "PurpleHaze";
        public override int StandTier => 3;
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
            Item.damage = 49;
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
                .AddIngredient(ModContent.ItemType<PurpleHazeT2>())
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 2)
                .AddIngredient(ItemID.FlaskofPoison, 3)
                .AddIngredient(ItemID.SoulofNight, 4)
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
