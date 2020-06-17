using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class HamonTrainingArmor : ModItem       //By Comobie
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hamon Training Armor");
            Tooltip.SetDefault("You can feel your lungs becoming mightier...\nIncreases Hamon Damage by 10%");
        }

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(0, 0, 75, 0);
            item.rare = ItemRarityID.Green;
			item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.1f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 20);
			recipe.AddIngredient(mod.ItemType("SunDroplet"), 25);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}