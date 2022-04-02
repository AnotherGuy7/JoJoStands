using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items.Seasonal
{
	public class GingerbreadArrow : ModItem
	{
		public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Stab yourself with this to for a 55% chance to give yourself a Christmas stand!.. or so it seemed?\nEat this arrow to gain a Christmas Stand.");
		}

		public override void SetDefaults()
        {
			item.width = 32;
			item.height = 32;
            item.useTime = 15;
            item.useAnimation = 15;
			item.maxStack = 1;
            item.useStyle = 3;
            item.noUseGraphic = true;
			item.rare = 8;
            item.consumable = true;
		}

        public override bool UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer)
				player.QuickSpawnItem(Main.rand.Next(JoJoStands.christmasStands.ToArray()));

            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
            recipe.AddIngredient(ItemID.GingerbreadCookie, 3);
			recipe.AddIngredient(ItemID.Ectoplasm, 2);
			recipe.AddIngredient(ItemID.Present, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}