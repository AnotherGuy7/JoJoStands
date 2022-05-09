using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

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
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 99;
            Item.value = Item.buyPrice(0, 0, 24, 0);
            Item.rare = ItemRarityID.Red;
		}
	}
}