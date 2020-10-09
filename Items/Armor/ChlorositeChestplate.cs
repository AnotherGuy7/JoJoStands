using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class ChlorositeChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorosite Chestplate");
            Tooltip.SetDefault("A chestplate that is made with Chlorophyte infused with an otherworldly virus.\n+7% Stand Crit Chance\n+6% Stand Damage");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 4, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.defense = 17;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standCritChangeBoosts += 7f;
            mPlayer.standDamageBoosts += 0.06f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ChlorositeBar"), 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}