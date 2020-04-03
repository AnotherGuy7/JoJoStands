using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items
{
	public class StandArrow : ModItem
	{
		public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Stab yourself with this to for a 55% chance to give yourself a stand!");
		}

		public override void SetDefaults()
        {
			item.width = 16;
			item.height = 16;
            item.useTime = 15;
            item.useAnimation = 15;
			item.maxStack = 1;
            item.useStyle = 3;
            item.noUseGraphic = true;
			item.rare = 8;
            item.consumable = true;
			item.value = Item.buyPrice(0, 10, 0, 0);
		}

        public override bool UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer)
			{
				if (Main.rand.Next(0, 101) > 107)
				{
					player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was deemed unworthy."), player.statLife + 1, player.direction);
				}
				else
				{
					//Item.NewItem(player.position, MyPlayer.standTier1List[Main.rand.Next(0, MyPlayer.standTier1List.Count)]);
					player.QuickSpawnItem(Main.rand.Next(MyPlayer.standTier1List.ToArray()));
				}
			}
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 4);
            recipe.AddIngredient(ItemID.Wood, 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
    }
}