using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class SealedPokerDeck : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sealed Poker Deck");
            Tooltip.SetDefault("Attack gets stronger every 5 seconds\nThis attack is a guaranteed crit");
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
            player.GetModPlayer<MyPlayer>().sealedPokerDeck = true;
        }
    }
}