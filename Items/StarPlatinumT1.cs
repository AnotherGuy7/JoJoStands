using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace JoJoStands.Items
{
	public class StarPlatinumT1 : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Platinum (Tier 1)");
			Tooltip.SetDefault("Punch enemies at a really fast rate.\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 22;
			item.width = 32;
			item.height = 32;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
            item.noUseGraphic = true;
			item.maxStack = 1;
			item.knockBack = 3f;
			item.value = 0;
			item.rare = 6;
        }

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
			TooltipLine tooltipAddition = new TooltipLine(mod, "Speed", "Punch Speed: " + (9 - mPlayer.standSpeedBoosts));
			tooltips.Add(tooltipAddition);
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.AddIngredient(mod.ItemType("WillToFight"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
