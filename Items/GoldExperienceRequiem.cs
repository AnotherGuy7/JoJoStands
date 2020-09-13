using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace JoJoStands.Items
{
	public class GoldExperienceRequiem : StandItemClass
	{
        public override int standSpeed => 9;
        public override int standType => 1;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Experience (Requiem)");
			Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to use abilities! \nSpecial: Switches the abilities used for right-click!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 138;
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GoldExperienceFinal"));
            recipe.AddIngredient(mod.ItemType("RequiemArrow"));
            recipe.AddIngredient(mod.ItemType("DeterminedLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
