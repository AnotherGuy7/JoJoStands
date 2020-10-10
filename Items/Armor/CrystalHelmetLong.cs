using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
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
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("CrystalChestplate") && legs.type == mod.ItemType("CrystalLeggings");
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
                item.type = mod.ItemType("CrystalHelmetNeutral");
                item.SetDefaults(mod.ItemType("CrystalHelmetNeutral"));
            }
            if (mPlayer.standType == 1)
            {
                item.type = mod.ItemType("CrystalHelmetShort");
                item.SetDefaults(mod.ItemType("CrystalHelmetShort"));
            }
        }
    }
}