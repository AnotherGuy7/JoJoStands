using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Chlorosite
{
    [AutoloadEquip(EquipType.Body)]
    public class ChlorositeChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorosite Chestplate");
            Tooltip.SetDefault("A chestplate that is made with Chlorophyte infused with an otherworldly virus.\n+7% Stand Crit Chance\n+6% Stand Damage");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 5, silver: 50);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 17;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standCritChangeBoosts += 7f;
            mPlayer.standDamageBoosts += 0.06f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ChlorositeBar>(), 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}