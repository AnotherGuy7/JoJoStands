using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class ViralBroadsword : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("A shiny sword that gives you the might to swing effortlessly...");
		}

		public override void SetDefaults()
		{
			item.damage = 27;
			item.width = 32;
			item.height = 32;
			item.useTime = 15;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.maxStack = 1;
			item.noUseGraphic = true;
			item.knockBack = 5f;
			item.rare = ItemRarityID.LightRed;
			item.shoot = mod.ProjectileType("ViralBroadswordProjectile");
			item.value = Item.buyPrice(0, 1, 25, 0);
			item.useTurn = true;
			item.noWet = true;
			item.channel = true;
			item.noMelee = true;
			item.autoReuse = false;
			item.melee = true;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			position = player.Center;
			if (Main.MouseWorld.X > player.Center.X)
			{
				player.direction = 1;
			}
			else
			{
				player.direction = -1;
			}
			speedX = 1f * player.direction;
			speedY = 0f;
			return true;
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
