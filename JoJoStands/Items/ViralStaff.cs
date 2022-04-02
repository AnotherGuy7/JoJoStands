using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class ViralStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("A gold and well designed staff.\nShoots enemy chasing orbs.");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 21;
			item.width = 24;
			item.height = 24;
			item.useAnimation = 25;
			item.useTime = 25;
			item.mana = 11;
			item.knockBack = 0f;
			item.magic = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.rare = ItemRarityID.LightRed;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.value = Item.buyPrice(0, 1, 25, 0);
			item.shoot = mod.ProjectileType("ViralStaffProjectile");
			item.shootSpeed = 32f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 shootVel = new Vector2(speedX, speedY);
			float numberProjectiles = 5;
			float rotation = MathHelper.ToRadians(30);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(shootVel.X + Main.rand.NextFloat(-6f, 6f), shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
				int proj = Projectile.NewProjectile(player.Center, perturbedSpeed, mod.ProjectileType("ViralStaffProjectile"), damage, knockBack, player.whoAmI);
				Main.projectile[proj].netUpdate = true;
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 3);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}
