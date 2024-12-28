using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Alloys
{
    [AutoloadEquip(EquipType.Head)]
    public class AdamantiteAlloyHelmetShort : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Adamantite Alloy Helmet (Short-Ranged)");
            // Tooltip.SetDefault("A helmet fused with Viral Meteorite to empower the user.\n8% Stand Crit Chance\n12% Stand Damage");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 20;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.AdamantiteBreastplate && legs.type == ItemID.AdamantiteLeggings;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+3 Stand Speed";
            player.GetModPlayer<MyPlayer>().standSpeedBoosts += 3;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standCritChangeBoosts += 8f;
            mPlayer.standDamageBoosts += 0.12f;

            if (mPlayer.standType == 0)
            {
                Item.type = ModContent.ItemType<AdamantiteAlloyHelmetNeutral>();
                Item.SetDefaults(ModContent.ItemType<AdamantiteAlloyHelmetNeutral>());
            }
            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<AdamantiteAlloyHelmetLong>();
                Item.SetDefaults(ModContent.ItemType<AdamantiteAlloyHelmetLong>());
            }
        }
    }
}