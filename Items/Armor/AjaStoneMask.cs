using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class AjaStoneMask : ModItem
	{
		public override void SetStaticDefaults() {
			Tooltip.SetDefault("The mask now merged with the stone seems to give off tremendous power...");
		}

		public override void SetDefaults() {
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(9, 0, 0, 0);
			item.rare = 10;
			item.defense = 20;
		}

        public override void UpdateEquip(Player player)
        {
            Lighting.AddLight(player.Center, 1f, 0f, 0f);
            player.AddBuff(mod.BuffType("AjaVampire"), 999999999);
        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StoneMask"));
			recipe.AddIngredient(mod.ItemType("AjaStone"));
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}