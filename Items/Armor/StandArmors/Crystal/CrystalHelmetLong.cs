using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Crystal
{
    [AutoloadEquip(EquipType.Head)]
    public class CrystalHelmetLong : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Helmet (Long-Ranged)");
            Tooltip.SetDefault("A helmet made to empower the force of the wills\n8% Stand Damage");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 5;
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
            mPlayer.standDamageBoosts += 0.08f;

            if (mPlayer.standType == 0)
            {
                Item.type = ModContent.ItemType<CrystalHelmetNeutral>();
                Item.SetDefaults(ModContent.ItemType<CrystalHelmetNeutral>());
            }
            if (mPlayer.standType == 1)
            {
                Item.type = ModContent.ItemType<CrystalHelmetShort>();
                Item.SetDefaults(ModContent.ItemType<CrystalHelmetShort>());
            }
        }
    }
}