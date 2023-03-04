using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CenturyBoyT2 : StandItemClass
    {
        public override string Texture
        {
            get { return Mod.Name + "/Items/CenturyBoyT1"; }
        }

        public override int StandTier => 2;
        public override string StandProjectileName => "CenturyBoy";
        public override Color StandTierDisplayColor => Color.Cyan;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("20th Century Boy (Tier 2)");
            Tooltip.SetDefault("Use the special ability key to make yourself immune to damage, but unable to move or use items.\nSpecial + Right-click: Set off an explosion! (Dynamite required)\nUsed in Stand Slot.");
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
                .AddIngredient(ModContent.ItemType<CenturyBoyT1>())
                .AddRecipeGroup("JoJoStandsCobalt-TierBar", 6)
                .AddIngredient(ItemID.HallowedBar, 4)
                .AddIngredient(ItemID.Dynamite, 5)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}