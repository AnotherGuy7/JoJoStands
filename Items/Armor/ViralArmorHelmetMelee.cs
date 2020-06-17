using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class ViralArmorHelmetMelee : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Helmet (Short-Range)");
            Tooltip.SetDefault("A helmet created from a far-off alloy, in the style of a far-off equipment.\nStand Speed Increase: -1");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = Item.buyPrice(0, 3, 50, 0);
            item.rare = ItemRarityID.Green;
            item.defense = 7;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ViralArmorKaruta") && legs.type == mod.ItemType("ViralArmorTabi");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+5% Stand Damage";
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.05;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standSpeedBoosts += 1;

            if (mPlayer.standType == 0)
            {
                item.type = mod.ItemType("ViralArmorHelmetNeutral");
                item.SetDefaults(mod.ItemType("ViralArmorHelmetNeutral"));
            }
            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("ViralArmorHelmetRanged");
                item.SetDefaults(mod.ItemType("ViralArmorHelmetRanged"));
            }
        }
    }
}