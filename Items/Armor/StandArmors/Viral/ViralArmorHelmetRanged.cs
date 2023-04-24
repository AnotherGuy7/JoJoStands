using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Viral
{
    [AutoloadEquip(EquipType.Head)]
    public class ViralArmorHelmetRanged : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Viral Helmet (Long-Range)");
            // Tooltip.SetDefault("A helmet created from a meteor, powered up by a strange virus.\nStand Damage Increase: +5%");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.value = Item.buyPrice(0, 3, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ViralArmorKaruta>() && legs.type == ModContent.ItemType<ViralArmorTabi>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+5% Stand Damage";
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.05f;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standDamageBoosts += 0.05f;

            if (mPlayer.standType == 0)
            {
                Item.type = ModContent.ItemType<ViralArmorHelmetNeutral>();
                Item.SetDefaults(ModContent.ItemType<ViralArmorHelmetNeutral>());
            }
            if (mPlayer.standType == 1)
            {
                Item.type = ModContent.ItemType<ViralArmorHelmetMelee>();
                Item.SetDefaults(ModContent.ItemType<ViralArmorHelmetMelee>());
            }
        }
    }
}