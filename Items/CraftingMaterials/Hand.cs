using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
    public class Hand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hand");
            Tooltip.SetDefault("A hand that a certain individual would kill for.");
            SacrificeTotal = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 22;
            Item.maxStack = 99;
            Item.value = Item.buyPrice(silver: 25);
            Item.rare = ItemRarityID.Yellow;
        }
    }
}