using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Front)]
    public class ArrowEarring : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Arrow Earring");
            /* Tooltip.SetDefault("When attacking enemies with Stand Attacks, 10% of damage against enemies is transmitted to nearby enemies." +
                "\nTransmitted damage is doubled if user not damaged for a long time."); */
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().arrowEarringEquipped = true;
        }
    }
}