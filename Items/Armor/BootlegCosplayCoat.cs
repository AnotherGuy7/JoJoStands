using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class BootlegCosplayCoat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bootleg Cosplay Coat");
            Tooltip.SetDefault("A helmet created from a far-off alloy, in the style of a far-off equipment.\nStand Critical Hit Chance Increase: +4%");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = Item.buyPrice(0, 0, 1, 0);
            item.rare = 8;
            item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 4f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(ItemID.Chain, 3);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 1);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}