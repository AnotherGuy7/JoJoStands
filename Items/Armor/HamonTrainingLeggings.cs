using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class HamonTrainingLeggings : ModItem        //By Comobie
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("You can feel a light rush in your legs...");
			DisplayName.SetDefault("Hamon Training Leggings");
		}

		public override void SetDefaults() {
			item.width = 18;
			item.height = 18;
            item.value = Item.buyPrice(0, 0, 65, 0);
			item.rare = 2;
			item.defense = 6;
		}

		public override void UpdateEquip(Player player) 
        {
			player.moveSpeed += 0.1f;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 15);
			recipe.AddIngredient(mod.ItemType("SunDroplet"), 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}