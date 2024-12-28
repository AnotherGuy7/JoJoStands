using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class UnderbossPhone : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Underboss Phone");
            /* Tooltip.SetDefault("Every fifth hit on enemies with Stand Attacks deal more damage." +
                "\nAttacks affected by this accessory ignore defense."); */
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SealedPokerDeck>();
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 2);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().underbossPhoneEquipped = true;
        }
    }
}