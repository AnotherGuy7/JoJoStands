using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TuskAct4 : StandItemClass
    {
        public override int StandSpeed => 12;
        public override int StandType => 2;
        public override int StandTier => 4;
        public override string StandIdentifierName => "TuskAct4";
        public static readonly Color TuskTierColor = new Color(255, 143, 214);
        public override Color StandTierDisplayColor => TuskTierColor;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tusk (ACT 4)");
            /* Tooltip.SetDefault("Use the infinite energy inside you..." +
                "\nWhile in Manual Mode: Left-click to deliver a flurry of punches with TA4." +
                "\nWhile in Auto Mode: Hold left-click to shoot and control a spinning nail and right-click to shoot an infinite spin nail!" +
                "\nSpecial: Switch to previous acts!" +
                "\nNote: To use Tusk Act 4, Spin Energy must be built up by first running at full speed with Slow Dancer." +
                "\nSlow Dancer can be summoned using the Slow Dancer's Saddle item."); */
        }

        public override void SetDefaults()
        {
            Item.damage = 305;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = 5;
            Item.maxStack = 1;
            Item.knockBack = 2f;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            mPlayer.standType = 2;
            mPlayer.equippedTuskAct = StandTier;
            mPlayer.tuskActNumber = 3;
            mPlayer.standName = "TuskAct4";
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TuskAct3>())
                .AddIngredient(ModContent.ItemType<RequiemArrow>())
                .AddIngredient(ModContent.ItemType<DeterminedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}