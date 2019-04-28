using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor
{
    public class CenturyBoy : ModItem
    {
        public void SetStaticDefault()
        {
            DisplayName.SetDefault("20th Century Boy");
            Tooltip.SetDefault("Use the special ability key to make yourself immune to damage, but unable to move or use items.");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.accessory = true;
            item.rare = 6;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (JoJoStands.AccessoryHotKey.Current)
            {
                player.controlUseItem = false;
                player.dash *= 0;
                player.bodyVelocity = new Vector2(0);
                player.controlLeft = false;
                player.controlJump = false;
                player.controlRight = false;
                player.controlDown = false;
                player.controlQuickHeal = false;
                player.controlQuickMana = false;
                player.controlUp = false;
                player.maxRunSpeed *= 0;
                player.moveSpeed *= 0;
                player.AddBuff(mod.BuffType("CenturyBoyBuff"), 1, true);
            }
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}