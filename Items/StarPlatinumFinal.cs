using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;

namespace JoJoStands.Items
{
	public class StarPlatinumFinal : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/StarPlatinumT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Platinum (Final)");
			Tooltip.SetDefault("Punch enemies at a really fast rate and use Star Finger to kill enemies from a distance. \nSpecial: Stop time for 4 seconds!\nUsed in Stand Slot");
		}

		public override void SetDefaults()
		{
			item.damage = 106;
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
			recipe.AddIngredient(mod.ItemType("StarPlatinumT3"));
            recipe.AddIngredient(ItemID.ChlorophyteOre, 15);
            recipe.AddIngredient(ItemID.LargeAmethyst, 1);
            recipe.AddIngredient(ItemID.FallenStar, 7);
			recipe.AddIngredient(mod.ItemType("RighteousLifeforce"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
