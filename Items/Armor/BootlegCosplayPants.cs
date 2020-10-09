using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class BootlegCosplayPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bootleg Cosplay Pants");
            Tooltip.SetDefault("Pants that, when worn, give you the feeling that you're someone else. Unfortunately, they didn't cost 100000 yen...\n+4% Stand Critical Hit Chance");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = Item.buyPrice(0, 0, 10, 0);
            item.rare = ItemRarityID.Blue;
            item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 4f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 15);
            recipe.AddIngredient(ItemID.IronBar, 5);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 15);
            recipe.AddIngredient(ItemID.LeadBar, 5);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}