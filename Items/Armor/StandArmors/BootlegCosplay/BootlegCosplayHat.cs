using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.BootlegCosplay
{
    [AutoloadEquip(EquipType.Head)]
    public class BootlegCosplayHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bootleg Cosplay Hat");
            Tooltip.SetDefault("A hat that tends to merge with your hair.\n+4% Stand Critical Hit Chance");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 18;
            Item.value = Item.buyPrice(silver: 4, copper: 50);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 4f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<BootlegCosplayCoat>() && legs.type == ModContent.ItemType<BootlegCosplayPants>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+10% Stand Damage";
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.IronBar, 3)
                .AddTile(TileID.Loom)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Silk, 10)
                .AddIngredient(ItemID.LeadBar, 3)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}