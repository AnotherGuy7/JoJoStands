using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items.Armor
{
    public class GoldAmuletOfFight : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 4));
            DisplayName.SetDefault("Gold Amulet of Fight");
            Tooltip.SetDefault("Wear this stone to walk on water, increase your regenerative capabilities, and increase your max hamon!");
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
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.15;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LargeRuby, 1);
            recipe.AddIngredient(ItemID.SunStone, 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
