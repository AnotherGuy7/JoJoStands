using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KingCrimsonFinal : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/KingCrimsonT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("King Crimson (Final Tier)");
			Tooltip.SetDefault("Donut enemies with a powerful punch and right-click to use Epitaph for 9 seconds!\nSpecial: Skip 10 seconds of time!\nUsed in Stand Slot");
		}

        public override void SetDefaults()
        {
            item.damage = 186;
            item.width = 32;
            item.height = 32;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 5f;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = 6;
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("KingCrimsonT3"));
            recipe.AddIngredient(ItemID.ChlorophyteBar, 13);
            recipe.AddIngredient(ItemID.GoldCrown);
            recipe.AddIngredient(mod.ItemType("SoulofTime"), 2);
            recipe.AddIngredient(mod.ItemType("WillToControl"), 3);
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 3);
            recipe.AddIngredient(mod.ItemType("TaintedLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
