using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class LuckPluck : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luck and Pluck");
            Tooltip.SetDefault("The sword of an old gladiator.");
        }

        public override void SetDefaults()
        {
            item.melee = true;
            item.damage = 14;
            item.width = 64;
            item.height = 30;
            item.knockBack = 5f;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.buyPrice(0, 0, 25, 0);
            item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 6);
            recipe.AddIngredient(ItemID.GoldBar, 4);
            recipe.AddIngredient(ItemID.Ruby);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
