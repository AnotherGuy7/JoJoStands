using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.VampirismArmors.IronSlop
{
    [AutoloadEquip(EquipType.Body)]
    public class IronSlopChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A chestplate filled with spots that look like clay.");
        }

        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 20;
            item.value = Item.buyPrice(silver: 10);
            item.rare = ItemRarityID.Blue;
            item.defense = 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 18);
            recipe.AddIngredient(ItemID.MudBlock, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LeadBar, 18);
            recipe.AddIngredient(ItemID.MudBlock, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}