using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.BootlegCosplay
{
    [AutoloadEquip(EquipType.Body)]
    public class BootlegCosplayCoat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bootleg Cosplay Coat");
            Tooltip.SetDefault("A coat that's the length of a robe but still counts as a coat.\n+1 Stand Speed");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standSpeedBoosts += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 20)
                .AddIngredient(ItemID.Chain, 3)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 1)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}