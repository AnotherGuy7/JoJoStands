using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AdamantiteAlloyHelmetLong : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adamantite Alloy Helmet (Long-Ranged)");
            Tooltip.SetDefault("A helmet fused with Viral Meteorite to empower the user.\n12% Stand Crit Chance\n18% Stand Damage");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.defense = 9;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemID.AdamantiteBreastplate && legs.type == ItemID.AdamantiteLeggings;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+10% Stand Crit Chance";
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 10;
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
            if (mPlayer.standType == 1)
            {
                item.type = mod.ItemType("AdamantiteAlloyHelmetShort");
                item.SetDefaults(mod.ItemType("AdamantiteAlloyHelmetShort"));
            }
        }
    }
}