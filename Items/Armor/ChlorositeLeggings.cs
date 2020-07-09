using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
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
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            player.moveSpeed *= 1.8f;
            mPlayer.standCritChangeBoosts += 9f;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ChlorositeBar"), 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}