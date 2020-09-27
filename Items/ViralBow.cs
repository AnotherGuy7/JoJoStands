using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class ViralBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("A gold and well designed staff.\nAll arrows shot become Viral Arrows chases down all enemies.");
		}

		public override void SetDefaults()
		{
			item.damage = 18;
			item.width = 24;
			item.height = 24;
			item.useAnimation = 30;
			item.useTime = 30;
			item.knockBack = 4f;
			item.ranged = true;
			item.noUseGraphic = false;
			item.UseSound = SoundID.Item1;
			item.rare = ItemRarityID.LightRed;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.value = Item.buyPrice(0, 1, 25, 0);
			item.shoot = mod.ProjectileType("ViralArrow");
			item.useAmmo = AmmoID.Arrow;
			item.shootSpeed = 8f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position, new Vector2(speedX, speedY), mod.ProjectileType("ViralArrow"), damage, knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 5);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}
