using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
	public class ViralPearl : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Pearl");
			Tooltip.SetDefault("A shiny red pearl that seems to react strongly to your stand.");
		}

		public override void SetDefaults()
        {
			item.width = 20;
			item.height = 20;
			item.maxStack = 99;
            item.value = Item.buyPrice(0, 0, 24, 0);
            item.rare = ItemRarityID.Red;
		}
	}
}