using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class RustyRevolver : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/PurpleRevolver"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rusty Revolver");
			Tooltip.SetDefault("An antique revolver, rusted by time. It still functions.");
		}

		public override void SetDefaults()
		{
			item.damage = 14;
			item.width = 30;
			item.height = 30;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 3f;
			item.value = Item.buyPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item41;
			item.autoReuse = false;
			item.ranged = true;
            item.shoot = 10;
            item.useAmmo = AmmoID.Bullet;
            item.maxStack = 1;
            item.shootSpeed = 14f;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 7);
            recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadBar, 7);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}