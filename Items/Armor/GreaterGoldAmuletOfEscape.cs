using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items.Armor
{
    public class GreaterGoldAmuletOfEscape : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 4));
            DisplayName.SetDefault("Greater Amulet of Escape");
            Tooltip.SetDefault("Increased Stand Attack Speed.");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 50, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standSpeedBoosts += 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Chain, 1);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 5);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.AddIngredient(mod.ItemType("GoldAmuletOfEscape"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
