using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.HamonArmors.Training
{
    [AutoloadEquip(EquipType.Head)]
    public class HamonTrainingHelmet : ModItem      //by Comobie
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hamon Training Hat");
            // Tooltip.SetDefault("A comfy hat that gives your head a clear concious...\nIncreases Hamon Damage by 5%");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.buyPrice(silver: 35);
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.05f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HamonTrainingArmor>() && legs.type == ModContent.ItemType<HamonTrainingLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Faster Hamon Regen";
            player.GetModPlayer<Hamon.HamonPlayer>().hamonIncreaseBonus += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 8)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}