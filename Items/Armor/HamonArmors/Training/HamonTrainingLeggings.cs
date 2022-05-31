using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.HamonArmors.Training
{
    [AutoloadEquip(EquipType.Legs)]
    public class HamonTrainingLeggings : ModItem        //By Comobie
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hamon Training Leggings");
            Tooltip.SetDefault("You can feel a light rush in your legs...\nIncreases Hamon Damage by 5%\nIncreases movement speed");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.1f;
            player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}