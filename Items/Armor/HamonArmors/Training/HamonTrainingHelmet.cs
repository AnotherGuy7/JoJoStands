using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.HamonArmors.Training
{
	[AutoloadEquip(EquipType.Head)]
	public class HamonTrainingHelmet : ModItem      //by Comobie
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hamon Training Hat");
			Tooltip.SetDefault("A comfy hat that gives your head a clear concious...\nIncreases Hamon Damage by 5%");
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.value = Item.buyPrice(0, 0, 50, 0);
            item.rare = ItemRarityID.Green;
			item.defense = 4;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.05f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("HamonTrainingArmor") && legs.type == mod.ItemType("HamonTrainingLeggings");
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Faster Hamon Regen";
			player.GetModPlayer<Hamon.HamonPlayer>().hamonIncreaseBonus += 1;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 8);
			recipe.AddIngredient(mod.ItemType("SunDroplet"), 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}