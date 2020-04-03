using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class StarPlatinumT2 : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/StarPlatinumT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Platinum (Tier 2)");
			Tooltip.SetDefault("Punch enemies at a really fast rate.\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
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

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StarPlatinumT1"));
            recipe.AddIngredient(ItemID.PlatinumBar, 12);
            recipe.AddIngredient(ItemID.FallenStar, 4);
			recipe.AddIngredient(mod.ItemType("WillToFight"));
			recipe.AddIngredient(mod.ItemType("WillToProtect"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StarPlatinumT1"));
            recipe.AddIngredient(ItemID.GoldBar, 12);
            recipe.AddIngredient(ItemID.FallenStar, 4);
			recipe.AddIngredient(mod.ItemType("WillToFight"));
			recipe.AddIngredient(mod.ItemType("WillToProtect"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
