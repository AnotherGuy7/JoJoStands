using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class HamonTrainingHelmet : ModItem      //by Comobie
	{
		public override void SetStaticDefaults() {
            Tooltip.SetDefault("A comfy hat that gives your head a clear concious...");
			DisplayName.SetDefault("Hamon Training Helmet");
		}

		public override void SetDefaults() {
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 0, 50, 0);
            item.rare = 2;
			item.defense = 4;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.1f;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 12);
			recipe.AddIngredient(mod.ItemType("SunDroplet"), 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}