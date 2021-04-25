using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Alloys
{
    [AutoloadEquip(EquipType.Head)]
    public class AdamantiteAlloyHelmetShort : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adamantite Alloy Helmet (Short-Ranged)");
            Tooltip.SetDefault("A helmet fused with Viral Meteorite to empower the user.\n8% Stand Crit Chance\n12% Stand Damage");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.defense = 20;
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
                item.type = mod.ItemType("AdamantiteAlloyHelmetNeutral");
                item.SetDefaults(mod.ItemType("AdamantiteAlloyHelmetNeutral"));
            }
            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("AdamantiteAlloyHelmetLong");
                item.SetDefaults(mod.ItemType("AdamantiteAlloyHelmetLong"));
            }
        }
    }
}