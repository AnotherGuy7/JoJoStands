using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.HamonArmors.Moorburn
{
	[AutoloadEquip(EquipType.Legs)]
	public class MoorburnGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moorburn Greaves");
			Tooltip.SetDefault("Greaves that were once the craft of a material evil.\nIncreases jump height");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
            item.value = Item.buyPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Green;
			item.defense = 6;
		}

		public override void UpdateEquip(Player player) 
        {
			player.moveSpeed += 0.1f;
			player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.05f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimtaneBar, 15);
			recipe.AddIngredient(mod.ItemType("SunDroplet"), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemoniteBar, 15);
			recipe.AddIngredient(mod.ItemType("SunDroplet"), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}