using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class ViralPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("A gold pickaxe that progressively increases mining speed.");
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 60;
			item.useAnimation = 60;
			item.pick = 70;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 3f;
			item.value = Item.buyPrice(0, 1, 25, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override bool UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				if (item.useTime > 5)
				{
					item.useTime--;
					item.useAnimation--;
				}
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 6);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}
