using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Items.Seasonal
{
	public class StarOnTheTree : StandItemClass
	{
		public override int standSpeed => 6;
		public override int standType => 1;
		public override int standTier => 4;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star On The Tree>();
			Tooltip.SetDefault("Left-click to punch enemies at a really fast rate and right-click to flick a bullet!\nIf there are no bullets in your inventory, Star Finger will be used instead.\nSpecial: Stop time for 4 seconds!\nUsed in Stand Slot>();
		}

		public override void SetDefaults()
		{
			Item.damage = 106;
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 1;
			Item.value = 0;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.LightPurple;
		}
		public override bool ManualStandSpawning(Player player)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.position, player.velocity, ModContent.ProjectileType<StarOnTheTreeStand>(), 0, 0f, Main.myPlayer);
			return true;
		}
	}
}
