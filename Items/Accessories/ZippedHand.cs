using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class ZippedHand : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zipped Hand");
            // Tooltip.SetDefault("Allows you to make a last stand before death.\nDuring the last stand phase, damage and critial strike chances are doubled.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 36;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().zippedHandEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("JoJoStandsGold-TierBar", 3)
                .AddIngredient(ModContent.ItemType<Hand>())
                .AddIngredient(ItemID.SoulofMight, 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}