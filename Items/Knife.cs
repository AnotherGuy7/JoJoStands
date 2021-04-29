using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class Knife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hunter's Knife");
			Tooltip.SetDefault("A sharp knife that is best suited to be thrown.");
		}
		public override void SetDefaults()
		{
			item.damage = 6;
			item.width = 9;
			item.height = 29;
			item.useTime = 16;
			item.useAnimation = 16;
			item.useStyle = 1;
            item.consumable = true;
            item.noUseGraphic = true;
			item.maxStack = 999;
			item.knockBack = 1f;
			item.value = Item.buyPrice(copper: 75);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
            item.shoot = mod.ProjectileType("Knife");
			item.shootSpeed = 25f;
		}

        public override bool UseItem(Player player)
        {
            player.ConsumeItem(mod.ItemType("Knife"));
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 3);
			recipe.SetResult(this, 75);
            recipe.AddTile(TileID.Anvils);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LeadBar, 3);
            recipe.SetResult(this, 75);
            recipe.AddTile(TileID.Anvils);
            recipe.AddRecipe();
        }
	}
}
