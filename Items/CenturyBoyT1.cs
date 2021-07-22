using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CenturyBoyT1 : StandItemClass
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("20th Century Boy (Tier 1)");
            Tooltip.SetDefault("Use the special ability key to make yourself immune to damage, but unable to move or use items.\nUsed in Stand Slot.");
        }

        public override int standTier => 1;

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StandArrow"));
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 3);
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 3);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standType = 1;
            mPlayer.showingCBLayer = true;
            mPlayer.standAccessory = true;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Networking.ModNetHandler.playerSync.SendCBLayer(256, player.whoAmI, true, player.whoAmI);
            }
            return true;
        }
    }
}