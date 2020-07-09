using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ChlorositeHelmetShort : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorosite Helmet (Short-Ranged)");
            Tooltip.SetDefault("A helmet that is made with Chlorophyte infused with an otherworldly virus.\n15% Stand Damage");
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
            return body.type == mod.ItemType("ChlorositeChestplate") && legs.type == mod.ItemType("ChlorositeLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+3 Stand Speed\nSummons a Crystal Leaf";
            player.GetModPlayer<MyPlayer>().standSpeedBoosts += 3;
            player.AddBuff(BuffID.LeafCrystal, 2);
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standCritChangeBoosts += 3f;
            mPlayer.standDamageBoosts += 0.15f;
            mPlayer.chlorositeShortEqquipped = true;

            if (mPlayer.standType == 0)
            {
                item.type = mod.ItemType("ChlorositeHelmetNeutral");
                item.SetDefaults(mod.ItemType("ChlorositeHelmetNeutral"));
            }
            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("ChlorositeHelmetLong");
                item.SetDefaults(mod.ItemType("ChlorositeHelmetLong"));
            }
        }
    }
}