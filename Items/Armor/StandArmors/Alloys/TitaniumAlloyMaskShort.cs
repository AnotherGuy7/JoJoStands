using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Alloys
{
    [AutoloadEquip(EquipType.Head)]
    public class TitaniumAlloyMaskShort : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Titanium Alloy Mask (Short-Ranged)");
            Tooltip.SetDefault("A mask fused with Viral Meteorite to empower the user.\n3 Stand Speed\n10% Stand Damage");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 22;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.TitaniumBreastplate && legs.type == ItemID.TitaniumLeggings;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Become immune after striking an enemy";
            player.GetModPlayer<MyPlayer>().wearingTitaniumMask = true;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standSpeedBoosts += 3;
            mPlayer.standDamageBoosts += 0.1f;

            if (mPlayer.standType == 0)
            {
                Item.type = ModContent.ItemType<TitaniumAlloyMaskNeutral>();
                Item.SetDefaults(ModContent.ItemType<TitaniumAlloyMaskNeutral>());
            }
            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<TitaniumAlloyMaskLong>();
                Item.SetDefaults(ModContent.ItemType<TitaniumAlloyMaskLong>());
            }
        }
    }
}