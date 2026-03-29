using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Front)]
    public class HazeVirusFilter : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Haze Virus Filter");
            // Tooltip.SetDefault("Grants permanent immunity to the Haze Virus and Concentrated Haze Virus.");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 5, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().hazeVirusFilter = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GreaterPlatinumAmuletOfEscape>())
                .AddIngredient(ModContent.ItemType<GreaterPlatinumAmuletOfChange>())
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 2)
                .AddIngredient(ItemID.Ectoplasm, 5)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}