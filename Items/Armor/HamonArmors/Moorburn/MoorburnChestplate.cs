using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.HamonArmors.Moorburn
{
	[AutoloadEquip(EquipType.Body)]
	public class MoorburnChestplate : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moorburn Chestplate");
            Tooltip.SetDefault("A heavy chestplate bathed in the energy of the Sun Droplets.\nIncreases Hamon Regen speed by 6%");
        }

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 24;
			item.value = Item.buyPrice(0, 1, 25, 0);
            item.rare = ItemRarityID.Green;
			item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.1f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimtaneBar, 20);
			recipe.AddIngredient(mod.ItemType("SunDroplet"), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemoniteBar, 20);
			recipe.AddIngredient(mod.ItemType("SunDroplet"), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}