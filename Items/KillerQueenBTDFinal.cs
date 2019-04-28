using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KillerQueenBTDFinal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Killer Queen (Stray Cat Final)");
			Tooltip.SetDefault("Shoot bubbles that explode and right-click to bite the dust!");
		}
		public override void SetDefaults()
		{
			item.damage = 201;
			item.ranged = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 6;
            item.UseSound = SoundID.Item85;
			item.autoReuse = false;
            item.shoot = mod.ProjectileType("Bubble");
			item.maxStack = 1;
            item.shootSpeed = 2f;
			item.channel = true;
		}

		public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
            {
                Main.time = 1600.0;
                item.useTime = 600;
                player.statLife += 500;
				player.Spawn();
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/BiteTheDustEffect"));
            }
			else
			{
				item.damage = 201;
				item.ranged = true;
				item.width = 10;
				item.height = 8;
				item.useTime = 60;
				item.useAnimation = 60;
				item.useStyle = 5;
				item.knockBack = 2;
				item.autoReuse = false;
	        	item.shoot = mod.ProjectileType("Bubble");
	            item.shootSpeed = 0.7f;
			}
			return true;
		}

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}