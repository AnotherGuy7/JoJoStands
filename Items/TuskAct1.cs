using System.Collections.Generic;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class TuskAct1 : StandItemClass
	{
        public override int standSpeed => 15;
        public override int standType => 2;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tusk (ACT 1)");
			Tooltip.SetDefault("Shoot nails at enemies!");
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

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}