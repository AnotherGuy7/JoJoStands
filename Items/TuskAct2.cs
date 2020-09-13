using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TuskAct2 : StandItemClass
    {
        public override int standSpeed => 35;
        public override int standType => 2;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tusk (ACT 2)");
            Tooltip.SetDefault("Shoot controllable spins at enemies!\nSpecial: Switch to previous acts!");
        }

        public override void SetDefaults()
        {
            item.damage = 49;
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
            recipe.AddIngredient(mod.ItemType("TuskAct1"));
            recipe.AddIngredient(ItemID.Hellstone, 12);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}