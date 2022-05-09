using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Chlorosite
{
    [AutoloadEquip(EquipType.Legs)]
    public class ChlorositeLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorosite Greaves");
            Tooltip.SetDefault("A couple of greaves that is made with Chlorophyte infused with an otherworldly virus.\n8% movement speed\n9% Stand Crit Chance");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            player.moveSpeed *= 1.8f;
            mPlayer.standCritChangeBoosts += 9f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ChlorositeBar>(), 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}