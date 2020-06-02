using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items.Armor
{
    public class IcedCup : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iced Water");
            Tooltip.SetDefault("Water with ice in it, perfect to stay hydrated and cool.");
        }
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 0, 25, 0);
            item.rare = ItemRarityID.Blue;
            item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddBuff(mod.BuffType("CooledOut"), 2);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(mod.ItemType("IceCubes"), 10);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
