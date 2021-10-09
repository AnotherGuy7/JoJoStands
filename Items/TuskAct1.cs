using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class TuskAct1 : StandItemClass
	{
        public override int standSpeed => 15;
        public override int standType => 2;
        public override int standTier => 1;


        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tusk (ACT 1)");
			Tooltip.SetDefault("Left-click to shoot nails at enemies and right-click to spin your nails in front of you to use as a melee weapon!");
		}

        public override void SetDefaults()
        {
            item.damage = 21;
            item.width = 32;
            item.height = 32;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2f;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            mPlayer.standType = 2;
            mPlayer.equippedTuskAct = standTier;
            mPlayer.tuskActNumber = standTier;
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.AddIngredient(mod.ItemType("WillToFight"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}