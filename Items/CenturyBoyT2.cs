using JoJoStands.Tiles;
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

        public override int standTier => 2;

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
            mPlayer.standType = 1;
            mPlayer.standName = "CenturyBoy";
            mPlayer.showingCBLayer = true;
            mPlayer.standAccessory = true;
            if (Main.netMode == NetmodeID.MultiplayerClient)
                Networking.ModNetHandler.playerSync.SendCBLayer(256, player.whoAmI, true, player.whoAmI);

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