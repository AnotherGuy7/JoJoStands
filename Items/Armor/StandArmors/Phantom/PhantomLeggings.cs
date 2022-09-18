using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Phantom
{
    [AutoloadEquip(EquipType.Legs)]
    public class PhantomLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Greaves");
            Tooltip.SetDefault("A couple of greaves that is made with ectoplasm and a strong virus.\n+10% Movement Speed\n+5% Stand Crit Chance");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 4, silver: 50);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.phantomLeggingsEquipped = true;
            mPlayer.standCritChangeBoosts += 5f;
            player.moveSpeed *= 1.10f;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Ectoplasm, 14)
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}