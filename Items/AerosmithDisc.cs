using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class AerosmithDisc : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aerosmith's Disc");
			Tooltip.SetDefault("Summon Aerosmith to help you shoot down enemies!");
		}

		public override void SetDefaults()
		{
			item.damage = 87;
			item.summon = true;
			item.width = 26;
			item.height = 28;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.noMelee = true;
			item.knockBack = 3;
			item.value = Item.buyPrice(2, 45, 0, 0);
			item.rare = 9;
			item.shoot = mod.ProjectileType("Aerosmith");
			item.shootSpeed = 10f;
			item.buffType = mod.BuffType("StarPlatinumBuff");
			item.buffTime = 3600;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
			return player.altFunctionUse != 2;
		}

		public override bool UseItem(Player player) {
			if (player.altFunctionUse == 2) {
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
		}
        
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SoulofLight, 54);
			recipe.AddIngredient(ItemID.SoulofSight, 15);
			recipe.AddIngredient(ItemID.Emerald, 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
