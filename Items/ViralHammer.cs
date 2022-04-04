using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class ViralHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("A hammer that, when wielded, hits with tremendous force.\nHurts enemies near the player when used.");
		}

		public override void SetDefaults()
		{
			item.damage = 21;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.hammer = 70;
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
				for (int n = 0; n < Main.maxNPCs; n++)
				{
					NPC npc = Main.npc[n];
					if (npc.active)
					{
						if (npc.Distance(player.position) <= 8f * 16f && npc.lifeMax > 5 && !npc.friendly && !npc.immortal && !npc.hide)
						{
							npc.StrikeNPC(18, 12f, player.direction);
						}
					}
				}
			}
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
