using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class PhantomLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Greaves");
            Tooltip.SetDefault("A couple of greaves that is made with ectoplasm and a strong virus.\n+10% Movement Speed\n+5% Stand Crit Chance");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.phantomLeggingsEquipped = true;
            mPlayer.standCritChangeBoosts += 5f;
            player.moveSpeed += player.moveSpeed * 1.08f;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Ectoplasm, 14);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 4);
            recipe.AddIngredient(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}