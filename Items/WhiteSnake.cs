using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class WhiteSnake : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("WhiteSnake");
		}
		public override void SetDefaults()
		{
			item.damage = 64;
			item.melee = true;
			item.width = 100;
			item.height = 8;
			item.useTime = 8;
			item.useAnimation = 8;
			item.useStyle = 3;
			item.maxStack = 1;
			item.knockBack = 2;
			item.value = 10000;
			item.rare = 6;
		}
	}
}
