using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items
{
	public class ArrowShard : ModItem
	{
		public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Stab yourself with this to slowly manifest a stand!");
		}

		public override void SetDefaults()
        {
			item.width = 16;
			item.height = 16;
            item.useTime = 15;
            item.useAnimation = 15;
			item.maxStack = 1;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.noUseGraphic = true;
			item.consumable = true;
			item.rare = ItemRarityID.Green;
            item.consumable = true;
			item.value = Item.buyPrice(0, 0, 5, 0);
		}

        public override bool UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer)
			{
				player.AddBuff(mod.BuffType("Pierced"), 36000);
				player.ConsumeItem(item.type);
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