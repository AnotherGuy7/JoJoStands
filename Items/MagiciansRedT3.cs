using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items
{
	public class MagiciansRedT3 : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/MagiciansRedT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magicians Red (Tier 3)");
			Tooltip.SetDefault("Shoot flaming ankhs at the enemies and right-click to grab an enemy!\nSpecial: Crossfire Hurricane!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 74;
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
            recipe.AddIngredient(mod.ItemType("MagiciansRedT2"));
            recipe.AddIngredient(mod.ItemType("WillToEscape"));
            recipe.AddIngredient(mod.ItemType("WillToProtect"));
            recipe.AddIngredient(ItemID.LivingFireBlock, 32);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
