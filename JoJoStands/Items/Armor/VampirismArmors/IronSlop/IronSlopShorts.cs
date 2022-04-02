using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors.IronSlop
{
    [AutoloadEquip(EquipType.Legs)]
    public class IronSlopShorts : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("War shorts made with iron? Allows you to feel the breeze!");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 14;
            item.value = Item.buyPrice(silver: 6);
            item.rare = ItemRarityID.Blue;
            item.defense = 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 12);
            recipe.AddIngredient(ItemID.MudBlock, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LeadBar, 12);
            recipe.AddIngredient(ItemID.MudBlock, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}