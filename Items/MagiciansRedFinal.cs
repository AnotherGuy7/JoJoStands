using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace JoJoStands.Items
{
	public class MagiciansRedFinal : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/MagiciansRedT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magicians Red (Final Tier)");
			Tooltip.SetDefault("Shoot flaming ankhs at the enemies and right-click to grab an enemy!\nSpecial: Crossfire Hurricane!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 95;
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

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            TooltipLine tooltipAddition = new TooltipLine(mod, "Speed", "Shoot Speed: " + (14 - mPlayer.standSpeedBoosts));
            tooltips.Add(tooltipAddition);
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("MagiciansRedT3"));
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddIngredient(ItemID.FireFeather);
            recipe.AddIngredient(mod.ItemType("CaringLifeforce"));
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
