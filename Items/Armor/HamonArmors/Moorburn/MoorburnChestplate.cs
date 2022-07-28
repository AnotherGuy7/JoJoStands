using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.HamonArmors.Moorburn
{
    [AutoloadEquip(EquipType.Body)]
    public class MoorburnChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moorburn Chestplate");
            Tooltip.SetDefault("A heavy chestplate bathed in the energy of the Sun Droplets.\nIncreases Hamon Regen speed by 6%");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 1, silver: 15);
            Item.rare = ItemRarityID.Green;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("JoJoStandsEvilBar", 20)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}