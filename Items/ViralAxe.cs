using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class ViralAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("An axe that seems to bind to the user.\nSpawns living infected wood sharpnels upon chopping a tree.");
		}

		public override void SetDefaults()
		{
			item.damage = 23;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.axe = 75 / 5;			//Whatever the value is it should be (that number / 5), cause weird vanilla stuff
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 3f;
			item.value = Item.buyPrice(0, 1, 25, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		private int shootCooldown = 0;

		public override void HoldItem(Player player)
		{
			if (shootCooldown > 0)
			{
				shootCooldown--;
			}
			if (Main.mouseLeft && shootCooldown <= 0)
			{
				float mouseDistance = player.Distance(Main.MouseWorld);
				if (Main.tile[(int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16].type == TileID.Trees && mouseDistance <= 5f * 16f)
				{
					shootCooldown += item.useTime;
					for (int p = 0; p < Main.rand.Next(3, 7); p++)
					{
						int proj = Projectile.NewProjectile(player.position, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)), mod.ProjectileType("ViralWoodSharpnel"), 23, 4f, player.whoAmI);
						Main.projectile[proj].netUpdate = true;
					}
				}
			}
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
