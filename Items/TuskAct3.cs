using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TuskAct3 : StandItemClass
    {
        public override int StandSpeed => 180;
        public override int StandType => 2;
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => TuskAct4.TuskTierColor;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tusk (ACT 3)");
            Tooltip.SetDefault("Hold left-click to shoot and control a spinning nail and right-click to shoot a slow wormhole to shoot out of!\nSpecial: Create a wormhole to travel in!\nSecond Special: Switch to previous acts!");
        }

        public override void SetDefaults()
        {
            Item.damage = 122;
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
            mPlayer.tuskActNumber = StandTier;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TuskAct2>())
                .AddIngredient(ItemID.HallowedBar, 11)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}