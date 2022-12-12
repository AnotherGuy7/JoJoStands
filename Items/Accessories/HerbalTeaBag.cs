using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class HerbalTeaBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Herbal Tea Bag");
            Tooltip.SetDefault("Next 10 ranged stand attacks will be buffed\nRecover one attack per second while at rest");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().herbalTeaBag = true;
        }
    }
}