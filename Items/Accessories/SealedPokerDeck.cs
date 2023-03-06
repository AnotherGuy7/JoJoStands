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
            Tooltip.SetDefault("Every 5 seconds, Stand Attacks deal 25% more damage.\nStand Attacks affected by this accessory are guaranteed critical strikes.");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().sealedPokerDeckEquipped = true;
        }
    }
}