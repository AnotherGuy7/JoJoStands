using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Phantom
{
    [AutoloadEquip(EquipType.Body)]
    public class PhantomChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Chestplate");
            Tooltip.SetDefault("A bright, glowing chestplate made with an adaptable virus and ectoplasm.\n+8% Stand Crit Chance\n+1 Stand Speed\n12% Incoming Damage Reduction");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 6);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.phantomChestplateEquipped = true;
            mPlayer.standCritChangeBoosts += 8f;
            mPlayer.standSpeedBoosts += 1;
            player.endurance += 0.12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Ectoplasm, 20)
                .AddIngredient(ModContent.ItemType<ViralPearl>())
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}