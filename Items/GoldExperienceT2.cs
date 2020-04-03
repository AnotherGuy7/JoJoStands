using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class GoldExperienceT2 : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/GoldExperienceFinal"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Experience (Tier 2)");
			Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to use abilities! \nSpecial: Switches the abilities used for right-click!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 41;
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
            recipe.AddIngredient(mod.ItemType("GoldExperienceT1"));
            recipe.AddIngredient(ItemID.GoldBar, 12);
            recipe.AddIngredient(ItemID.Acorn, 20);
            recipe.AddIngredient(ItemID.LifeCrystal);
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GoldExperienceT1"));
            recipe.AddIngredient(ItemID.PlatinumBar, 12);
            recipe.AddIngredient(ItemID.Acorn, 20);
            recipe.AddIngredient(ItemID.LifeCrystal);
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
