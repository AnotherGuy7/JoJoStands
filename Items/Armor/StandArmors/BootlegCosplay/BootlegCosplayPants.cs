using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.BootlegCosplay
{
    [AutoloadEquip(EquipType.Legs)]
    public class BootlegCosplayPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bootleg Cosplay Pants");
            // Tooltip.SetDefault("Pants that, when worn, give you the feeling that you're someone else. Unfortunately, they didn't cost 100000 yen...\n+4% Stand Critical Hit Chance");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.value = Item.buyPrice(silver: 3, copper: 50);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 4f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 15)
                .AddIngredient(ItemID.IronBar, 5)
                .AddTile(TileID.Loom)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Silk, 15)
                .AddIngredient(ItemID.LeadBar, 5)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}