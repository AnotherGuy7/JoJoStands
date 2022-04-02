using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.BootlegCosplay
{
    [AutoloadEquip(EquipType.Body)]
    public class BootlegCosplayCoat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bootleg Cosplay Coat");
            Tooltip.SetDefault("A coat that's the length of a robe but still counts as a coat.\n+1 Stand Speed");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = Item.buyPrice(0, 0, 10, 0);
            item.rare = ItemRarityID.Blue;
            item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standSpeedBoosts += 1;
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