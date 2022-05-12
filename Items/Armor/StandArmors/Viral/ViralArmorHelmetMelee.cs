using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Viral
{
    [AutoloadEquip(EquipType.Head)]
    public class ViralArmorHelmetMelee : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Helmet (Short-Range)");
            Tooltip.SetDefault("A helmet created from a far-off alloy, in the style of a far-off equipment.\nStand Speed Increase: -1");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 3, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 7;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ViralArmorKaruta>() && legs.type == ModContent.ItemType<ViralArmorTabi>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+5% Stand Damage";
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.05f;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standSpeedBoosts += 1;

            if (mPlayer.standType == 0)
            {
                Item.type = ModContent.ItemType<ViralArmorHelmetNeutral>();
                Item.SetDefaults(ModContent.ItemType<ViralArmorHelmetNeutral>());
            }
            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<ViralArmorHelmetRanged>();
                Item.SetDefaults(ModContent.ItemType<ViralArmorHelmetRanged>());
            }
        }
    }
}