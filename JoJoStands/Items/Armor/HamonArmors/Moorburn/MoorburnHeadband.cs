using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.HamonArmors.Moorburn
{
	[AutoloadEquip(EquipType.Head)]
	public class MoorburnHeadband : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moorburn Headband");
			Tooltip.SetDefault("A headband purified by Hamon.\nIncreases Hamon Damage by 12%");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 10;
			item.value = Item.buyPrice(0, 0, 55, 50);
            item.rare = ItemRarityID.Green;
			item.defense = 4;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.12f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("MoorburnChestplate") && legs.type == mod.ItemType("MoorburnGreaves");
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "+5% Movement Speed\nWhile holding a Hamon Item: Defense is increased by 5.";
			player.moveSpeed *= 1.05f;
			if (player.HeldItem.modItem is Hamon.HamonDamageClass)
				player.statDefense += 5;
		}

		public override void AddRecipes()
        {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimtaneBar, 6);
			recipe.AddIngredient(mod.ItemType("SunDroplet"), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemoniteBar, 6);
			recipe.AddIngredient(mod.ItemType("SunDroplet"), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}