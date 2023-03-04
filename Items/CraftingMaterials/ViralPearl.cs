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
            Item.width = 22;
            Item.height = 22;
            Item.maxStack = 99;
            Item.value = Item.buyPrice(silver: 40);
            Item.rare = ItemRarityID.Red;
        }
    }
}