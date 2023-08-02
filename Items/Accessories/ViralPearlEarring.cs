using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class ViralPearlEarring : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Viral Pearl Earring");
            /* Tooltip.SetDefault("When attacking enemies with Stand Attacks, 10% of damage against enemies is transmitted to nearby enemies." +
                "\nTransmitted damage is doubled if user not damaged for a long time.\nPunching enemies with Stands can infect enemies with a virus."); */
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 38;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 50);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().arrowEarringEquipped = true;
            player.GetModPlayer<MyPlayer>().crackedPearlEquipped = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ArrowEarring>())
                .AddIngredient(ModContent.ItemType<CrackedPearl>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}