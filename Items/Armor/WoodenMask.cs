using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class WoodenMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("An wooden mask based off of a much more menacing mask. When worn, you gain superhuman abilities, but you burn in the sunlight.");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 44;
            item.value = Item.buyPrice(silver: 5);
            item.rare = ItemRarityID.Green;
            item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddBuff(mod.BuffType("Zombie"), 2);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 40);
            recipe.AddIngredient(ItemID.GoldBar, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 40);
            recipe.AddIngredient(ItemID.PlatinumBar, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}