using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CenturyBoyT1 : StandItemClass
    {
        public override int StandTier => 1;
        public override string StandProjectileName => "CenturyBoy";
        public override Color StandTierDisplayColor => Color.Cyan;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("20th Century Boy (Tier 1)");
            // Tooltip.SetDefault("Use the special ability key to make yourself immune to damage, but unable to move or use items.\nUsed in Stand Slot.");
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 48;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standType = 2;
            mPlayer.standName = "CenturyBoy";
            mPlayer.standAccessory = true;

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 3)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 3)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}