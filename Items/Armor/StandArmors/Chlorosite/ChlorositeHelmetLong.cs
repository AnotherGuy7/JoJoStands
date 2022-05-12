using JoJoStands.Buffs.AccessoryBuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Chlorosite
{
    [AutoloadEquip(EquipType.Head)]
    public class ChlorositeHelmetLong : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorosite Helmet (Long-Ranged)");
            Tooltip.SetDefault("A helmet that is made with Chlorophyte infused with an otherworldly virus.\n15% Stand Damage");
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
            return body.type == ModContent.ItemType<ChlorositeChestplate>() && legs.type == ModContent.ItemType<ChlorositeLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+5% Stand Damage\nSummons a Viral Crystal";
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.05f;
            player.AddBuff(ModContent.BuffType<ViralCrystalBuff>(), 2);
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standDamageBoosts += 0.15f;

            if (mPlayer.standType == 0)
            {
                Item.type = ModContent.ItemType<ChlorositeHelmetNeutral>();
                Item.SetDefaults(ModContent.ItemType<ChlorositeHelmetNeutral>());
            }
            if (mPlayer.standType == 1)
            {
                Item.type = ModContent.ItemType<ChlorositeHelmetShort>();
                Item.SetDefaults(ModContent.ItemType<ChlorositeHelmetShort>());
            }
        }
    }
}