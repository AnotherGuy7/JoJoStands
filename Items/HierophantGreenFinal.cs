using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace JoJoStands.Items
{
	public class HierophantGreenFinal : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/HierophantGreenT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hierophant Green (Final)");
			Tooltip.SetDefault("Shoot emeralds at the enemies and right click to shoot more accurate emralds!\nSpecial: 20 Meter Emerald Splash!\nUsed in Stand Slot");
		}

        public override void SetDefaults()
        {
            item.damage = 72;
            item.width = 32;
            item.height = 32;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 3f;
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
			recipe.AddIngredient(mod.ItemType("HierophantGreenT2"));
            recipe.AddIngredient(ItemID.LargeEmerald, 1);
            recipe.AddIngredient(ItemID.ChlorophyteOre, 12);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 3);
            recipe.AddIngredient(mod.ItemType("WillToChange"), 3);
            recipe.AddIngredient(mod.ItemType("RighteousLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
