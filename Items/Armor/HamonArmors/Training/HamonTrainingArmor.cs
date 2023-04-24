using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.HamonArmors.Training
{
    [AutoloadEquip(EquipType.Body)]
    public class HamonTrainingArmor : ModItem       //By Comobie
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hamon Training Vest");
            // Tooltip.SetDefault("You can feel your lungs becoming mightier...\nIncreases Hamon Damage by 10%");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 22;
            Item.value = Item.buyPrice(silver: 65);
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 15)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 7)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}