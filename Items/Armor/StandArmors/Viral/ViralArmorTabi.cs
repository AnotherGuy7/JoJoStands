using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Viral
{
	[AutoloadEquip(EquipType.Legs)]
	public class ViralArmorTabi : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Tabi");
			Tooltip.SetDefault("A pair of light boots from a surprisingly nimble meteor, powered by a strange virus.\nProvides a 4% Stand Crit Chance boost");
		}

		public override void SetDefaults()
        {
			item.width = 22;
			item.height = 16;
			item.value = Item.buyPrice(0, 3, 50, 0);
			item.rare = ItemRarityID.Green;
			item.defense = 5;
		}

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 4f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 25);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}