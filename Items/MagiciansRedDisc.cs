using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class MagiciansRedDisc : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magician Red's Disc");
			Tooltip.SetDefault("Summon Magicians Red to help you fight!");
		}

		public override void SetDefaults()
		{
			item.summon = true;
			item.width = 26;
			item.height = 28;
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 1;
			item.noMelee = true;
			item.value = Item.buyPrice(2, 45, 0, 0);
			item.rare = 9;
			item.shoot = mod.ProjectileType("MagiciansRedMinion");
			item.shootSpeed = 10f;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
			return player.altFunctionUse != 2;
		}

		public override bool UseItem(Player player) {
			if (player.altFunctionUse == 2)
            {
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
		}
	}
}
