using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Crystal
{
    [AutoloadEquip(EquipType.Body)]
    public class CrystalChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Chestplate");
            Tooltip.SetDefault("A shiny chestplate that uses crystals to sharpen your soul...\n+10% Stand Crit Chance");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 2, silver: 75);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrystalShard, 15)
                .AddIngredient(ItemID.Silk, 15)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}