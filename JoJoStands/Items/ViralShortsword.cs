using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class ViralShortsword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("A shiny but tiny sword that gives you the speed and strength to dash away!");
		}

		public override void SetDefaults()
		{
			item.damage = 27;
			item.width = 32;
			item.height = 32;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = ItemUseStyleID.Stabbing;
			item.UseSound = SoundID.Item1;
			item.maxStack = 1;
			item.knockBack = 5f;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.buyPrice(0, 1, 25, 0);
			item.useTurn = true;
			item.autoReuse = true;
			item.melee = true;
		}

		public override void HoldItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				if (Main.mouseLeft)
				{
					if (Math.Abs(player.velocity.X) <= 12f)			//Abs is absolute value
					{
						player.velocity.X += 0.4f * player.direction;
						item.knockBack = Math.Abs(player.velocity.X) * 2f;
					}
				}
			}
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			flat = Math.Abs(player.velocity.X) * 2f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 4);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}
