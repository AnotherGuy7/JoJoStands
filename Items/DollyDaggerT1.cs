using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class DollyDaggerT1 : StandItemClass
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dolly Dagger (Tier 1)");
			Tooltip.SetDefault("As an item: Left-click to use this as a dagger to stab enemies!\nIn the Stand Slot: Equip it to nullify and reflect 35% of all damage!");
		}

		public override void SetDefaults()
		{
			item.damage = 34;
			item.width = 16;
			item.height = 16;
			item.useTime = 15;
			item.useAnimation = 15;
			item.maxStack = 1;
			item.noUseGraphic = false;
			item.useStyle = ItemUseStyleID.Stabbing;
			item.UseSound = SoundID.Item1;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.buyPrice(0, 2, 0, 0);
		}

		public override void RightClick(Player player)
		{ }

        public override bool ManualStandSpawning(Player player)
        {
			MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
			mPlayer.standType = 1;
			mPlayer.standAccessory = true;
			player.AddBuff(mod.BuffType("DollyDaggerActiveBuff"), 10);
			return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 4);
			recipe.AddIngredient(ItemID.Wood, 3);
			recipe.AddIngredient(mod.ItemType("WillToChange"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}