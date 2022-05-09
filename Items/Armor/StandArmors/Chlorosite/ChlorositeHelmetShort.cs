using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace JoJoStands.Items.Armor.StandArmors.Chlorosite
{
    [AutoloadEquip(EquipType.Head)]
    public class ChlorositeHelmetShort : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorosite Helmet (Short-Ranged)>());
            Tooltip.SetDefault("A helmet that is made with Chlorophyte infused with an otherworldly virus.\n15% Stand Damage>());
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
            return body.type == ModContent.ItemType<ChlorositeChestplate>()) && legs.type == ModContent.ItemType<ChlorositeLeggings>());
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+3 Stand Speed\nSummons a Viral Crystal";
            player.GetModPlayer<MyPlayer>()).standSpeedBoosts += 3;
            player.AddBuff(ModContent.BuffType<ViralCrystalBuff>()), 2);
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>());
            mPlayer.standCritChangeBoosts += 3f;
            mPlayer.standDamageBoosts += 0.15f;
            mPlayer.chlorositeShortEqquipped = true;

            if (mPlayer.standType == 0)
            {
                Item.type = ModContent.ItemType<ChlorositeHelmetNeutral>());
                Item.SetDefaults(ModContent.ItemType<ChlorositeHelmetNeutral>()));
            }
            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<ChlorositeHelmetLong>());
                Item.SetDefaults(ModContent.ItemType<ChlorositeHelmetLong>()));
            }
        }
    }
}