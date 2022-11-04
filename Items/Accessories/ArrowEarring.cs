using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class ArrowEarring : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arrow Earring");
            Tooltip.SetDefault("10% of damage aganist an enemy is transmitted to nearby creatures\nTransmitted damage is doubled if user not damaged for a long time");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().arrowEarring = true;
        }
    }
}