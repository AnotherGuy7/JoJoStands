using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KingCrimsonT1 : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("King Crimson (Tier 1)");
			Tooltip.SetDefault("Donut enemies with a powerful punch!\nUsed in Stand Slot");
		}

        public override void SetDefaults()      ///30 reuseTime
        {
            item.damage = 42;
            item.width = 32;
            item.height = 32;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2f;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = 6;
            MyPlayer.standTier1List.Add(mod.ItemType(Name));
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.AddIngredient(mod.ItemType("WillToControl"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
