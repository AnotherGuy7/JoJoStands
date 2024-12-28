using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StandArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Stab yourself with this to for a 55% chance to give yourself a stand!");
            Item.ResearchUnlockCount = 2;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.maxStack = 1;
            Item.useStyle = 3;
            Item.noUseGraphic = true;
            Item.rare = 8;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 10, 0, 0);
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (Main.rand.Next(0, 101) <= 55)
                    player.QuickSpawnItem(player.GetSource_FromThis(), Main.rand.Next(JoJoStands.standTier1List.ToArray()));
                else
                    player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was deemed unworthy."), player.statLife + 1, player.direction);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 4)
                .AddIngredient(ItemID.Wood, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}