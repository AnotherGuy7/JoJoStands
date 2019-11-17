using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items
{
	public class ViralMeteoriteBar : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Meteorite Bar");
            Tooltip.SetDefault("You feel your soul interacting with the metal.");

        }

		public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 3, 0, 25);
		}

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ViralMeteorite"), 15);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}