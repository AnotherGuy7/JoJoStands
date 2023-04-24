using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.HamonArmors.Moorburn
{
    [AutoloadEquip(EquipType.Legs)]
    public class MoorburnGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Moorburn Greaves");
            // Tooltip.SetDefault("Greaves that were once the craft of a material evil.\nIncreases jump height");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(silver: 80);
            Item.rare = ItemRarityID.Green;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.1f;
            player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("JoJoStandsEvilBar", 15)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}