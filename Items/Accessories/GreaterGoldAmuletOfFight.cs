using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items.Accessories
{
    public class GreaterGoldAmuletOfFight : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 4));
            DisplayName.SetDefault("Greater Amulet of Fight");
            Tooltip.SetDefault("20% increased Stand attack damage");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.2f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chain, 1);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddIngredient(mod.ItemType("WillToFight"), 5);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.AddIngredient(mod.ItemType("GoldAmuletOfFight"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
