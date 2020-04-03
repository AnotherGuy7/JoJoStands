using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KillerQueenT3 : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/KillerQueenT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Killer Queen (1st Bomb Tier 3)");
			Tooltip.SetDefault("Left-click to punch and right-click to trigger any block! \nRange: 14 blocks \nSpecial: Sheer Heart Attack!\nUsed in Stand Slot");
		}

        public override void SetDefaults()
        {
            item.damage = 59;
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
            recipe.AddIngredient(mod.ItemType("KillerQueenT2"));
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddIngredient(mod.ItemType("Hand"), 1);
            recipe.AddIngredient(ItemID.SoulofMight, 8);
            recipe.AddIngredient(mod.ItemType("WillToDestroy"), 2);
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}