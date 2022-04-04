using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Seasonal
{
	public class StarOnTheTree : StandItemClass
	{
		public override int standSpeed => 6;
		public override int standType => 1;
		public override int standTier => 4;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star On The Tree");
			Tooltip.SetDefault("Left-click to punch enemies at a really fast rate and right-click to flick a bullet!\nIf there are no bullets in your inventory, Star Finger will be used instead.\nSpecial: Stop time for 4 seconds!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 106;
			item.width = 32;
			item.height = 32;
			item.maxStack = 1;
			item.value = 0;
			item.noUseGraphic = true;
			item.rare = ItemRarityID.LightPurple;
		}
		public override bool ManualStandSpawning(Player player)
		{
			Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("StarOnTheTreeStand"), 0, 0f, Main.myPlayer);
			return true;
		}
	}
}
