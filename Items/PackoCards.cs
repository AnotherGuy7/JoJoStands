using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class PackoCards : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pack'o'Cards");
			Tooltip.SetDefault("D'Arby's favorite set of cards... Just throw them at enemies");
		}

		public override void SetDefaults()
		{
			item.damage = 102;
			item.ranged = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.knockBack = 2;
			item.value = Item.buyPrice(0, 23, 0, 0);
            item.noUseGraphic = true;
			item.rare = 8;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Card");
			item.maxStack = 1;
            item.shootSpeed = 36f;
		}
	}
}
