using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using JoJoStands.Buffs.ItemBuff;

namespace JoJoStands.Items
{
	public class IceCubes : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Cubes");
			Tooltip.SetDefault("Ice cubes that are in perfect size for eating.");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 999;
			item.value = Item.buyPrice(0, 0, 0, 60);
			item.rare = ItemRarityID.Orange;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.UseSound = SoundID.Item2;
			item.consumable = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useTurn = true;
			item.buffType = mod.BuffType("CooledOut");
			item.buffTime = 3000;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IceBlock, 1);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
}