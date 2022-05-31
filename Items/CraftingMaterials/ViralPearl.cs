using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
    public class ViralPearl : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Pearl");
            Tooltip.SetDefault("A shiny red pearl that seems to react strongly to your stand.");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 99;
            Item.value = Item.buyPrice(silver: 40);
            Item.rare = ItemRarityID.Red;
        }
    }
}