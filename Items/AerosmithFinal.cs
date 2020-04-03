using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items
{
	public class AerosmithFinal : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/AerosmithT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aerosmith (Final Tier)");
			Tooltip.SetDefault("Left-click to move and right-click to shoot bullets at the enemies!\nSpecial: Drop a bomb on enemies!\nPassive: Carbon Radar\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 75;
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
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AerosmithT3"));
            recipe.AddIngredient(ItemID.ShroomiteBar, 10);
            recipe.AddIngredient(mod.ItemType("CaringLifeforce"));
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 2);
            recipe.AddIngredient(ItemID.MartianConduitPlating, 75);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
