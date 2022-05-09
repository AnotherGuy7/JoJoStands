using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace JoJoStands.Items.Armor.StandArmors.Crystal
{
    [AutoloadEquip(EquipType.Head)]
    public class CrystalHelmetShort : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Helmet (Short-Ranged)>();
            Tooltip.SetDefault("A helmet made to empower the force of the wills\n+2 Stand Speed>();
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 12;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<CrystalChestplate>() && legs.type == ModContent.ItemType<CrystalLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+8% Stand Damage; Crystal Shards are released when hit";
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.08f;
            player.GetModPlayer<MyPlayer>().crystalArmorSetEquipped = true;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standSpeedBoosts += 2;

            if (mPlayer.standType == 0)
            {
                Item.type = ModContent.ItemType<CrystalHelmetNeutral>();
                Item.SetDefaults(ModContent.ItemType<CrystalHelmetNeutral>());
            }
            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<CrystalHelmetLong>();
                Item.SetDefaults(ModContent.ItemType<CrystalHelmetLong>());
            }
        }
    }
}