using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class HamonTrainingLeggings : ModItem        //By Comobie
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hamon Training Leggings");
			Tooltip.SetDefault("You can feel a light rush in your legs...\nIncreases Hamon Damage by 5%\nIncreases movement speed");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
            item.value = Item.buyPrice(0, 0, 65, 0);
			item.rare = ItemRarityID.Green;
			item.defense = 4;
		}

		public override void UpdateEquip(Player player) 
        {
			player.moveSpeed += 0.1f;
			player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.05f;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 15);
			recipe.AddIngredient(mod.ItemType("SunDroplet"), 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}