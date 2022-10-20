using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Alloys
{
    [AutoloadEquip(EquipType.Head)]
    public class TitaniumAlloyMaskLong : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Titanium Alloy Mask (Long-Ranged)");
            Tooltip.SetDefault("A mask fused with Viral Meteorite to empower the user.\n10% Stand Crit Chance\n15% Stand Damage");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.TitaniumBreastplate && legs.type == ItemID.TitaniumLeggings;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Attacking generates a defensive barrier of titanium shards";
            player.onHitTitaniumStorm = true;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standCritChangeBoosts += 15f;
            mPlayer.standDamageBoosts += 0.15f;

            if (mPlayer.standType == 0)
            {
                Item.type = ModContent.ItemType<TitaniumAlloyMaskNeutral>();
                Item.SetDefaults(ModContent.ItemType<TitaniumAlloyMaskNeutral>());
            }
            if (mPlayer.standType == 1)
            {
                Item.type = ModContent.ItemType<TitaniumAlloyMaskShort>();
                Item.SetDefaults(ModContent.ItemType<TitaniumAlloyMaskShort>());
            }
        }
    }
}