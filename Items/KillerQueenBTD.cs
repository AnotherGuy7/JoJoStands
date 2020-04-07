using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace JoJoStands.Items
{
	public class KillerQueenBTD : ModItem
	{
        public static bool taggedAnything = false;

        public override string Texture
        {
            get { return mod.Name + "/Items/KillerQueenT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Killer Queen (Stray Cat)");
			Tooltip.SetDefault("Left-click to shoot bubbles and right-click to detonate them!\nSpecial: Bite The Dust!\nRight-Click while holding the item to revert back to Killer Queen Final (You can revert back to BTD)\nUsed in Stand Slot");
		}

        public override void SetDefaults()
        {
            item.damage = 180;
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
            TooltipLine tooltipAddition = new TooltipLine(mod, "Speed", "Shoot Speed: " + (60 - mPlayer.standSpeedBoosts));
            tooltips.Add(tooltipAddition);
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
        }

        public override void OnCraft(Recipe recipe)
        {
            Main.LocalPlayer.GetModPlayer<MyPlayer>().canRevertFromKQBTD = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return player.GetModPlayer<MyPlayer>().canRevertFromKQBTD;
        }

        public override bool CanUseItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.altFunctionUse == 2 && mPlayer.revertTimer <= 0)
            {
                item.TurnToAir();
                Item.NewItem(player.Center, mod.ItemType("KillerQueenFinal"));
                mPlayer.revertTimer += 30;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("KillerQueenFinal"));
            recipe.AddIngredient(mod.ItemType("RequiemArrow"));
            recipe.AddIngredient(mod.ItemType("TaintedLifeforce"));
            recipe.AddIngredient(mod.ItemType("StrayCat"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}