using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ViralArmorHelmetRanged : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Helmet (Long-Range)");
            Tooltip.SetDefault("A helmet created from a meteor, powered up by a strange virus.\nStand Damage Increase: +5%");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.value = Item.buyPrice(0, 3, 50, 0);
            item.rare = ItemRarityID.Green;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ViralArmorKaruta") && legs.type == mod.ItemType("ViralArmorTabi");
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
                item.type = mod.ItemType("ViralArmorHelmetNeutral");
                item.SetDefaults(mod.ItemType("ViralArmorHelmetNeutral"));
            }
            if (mPlayer.standType == 1)
            {
                item.type = mod.ItemType("ViralArmorHelmetMelee");
                item.SetDefaults(mod.ItemType("ViralArmorHelmetMelee"));
            }
        }
    }
}