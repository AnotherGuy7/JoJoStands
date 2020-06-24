using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
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
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.defense = 22;
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
                item.type = mod.ItemType("TitaniumAlloyMaskNeutral");
                item.SetDefaults(mod.ItemType("TitaniumAlloyMaskNeutral"));
            }
            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("TitaniumAlloyMaskLong");
                item.SetDefaults(mod.ItemType("TitaniumAlloyMaskLong"));
            }
        }
    }
}