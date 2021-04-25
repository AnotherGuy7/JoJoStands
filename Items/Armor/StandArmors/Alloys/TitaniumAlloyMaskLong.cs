using Terraria.ID;
using Terraria;
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
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.defense = 10;
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
            mPlayer.standCritChangeBoosts += 15f;
            mPlayer.standDamageBoosts += 0.15f;

            if (mPlayer.standType == 0)
            {
                item.type = mod.ItemType("TitaniumAlloyMaskNeutral");
                item.SetDefaults(mod.ItemType("TitaniumAlloyMaskNeutral"));
            }
            if (mPlayer.standType == 1)
            {
                item.type = mod.ItemType("TitaniumAlloyMaskShort");
                item.SetDefaults(mod.ItemType("TitaniumAlloyMaskShort"));
            }
        }
    }
}