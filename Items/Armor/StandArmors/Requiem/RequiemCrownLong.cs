using JoJoStands.Buffs.AccessoryBuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Requiem
{
    [AutoloadEquip(EquipType.Head)]
    public class RequiemCrownLong : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Requiem Crown (Long-Ranged)");
            // Tooltip.SetDefault("A crown made from the finest materials space has offered so far.\n+18% Stand Crit Chance\n+22% Stand Damage\n+1 Stand Speed");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.defense = 15;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<RequiemChestplate>() && legs.type == ModContent.ItemType<RequiemGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Viral Beetles float around you, defending you from anything that comes your way.";
            player.AddBuff(ModContent.BuffType<ViralBeetleBuff>(), 2);
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standCritChangeBoosts += 18f;
            mPlayer.standDamageBoosts += 0.22f;
            mPlayer.standSpeedBoosts += 1;

            if (mPlayer.standType == 0)
            {
                Item.type = ModContent.ItemType<RequiemCrownNeutral>();
                Item.SetDefaults(ModContent.ItemType<RequiemCrownNeutral>());
            }
            if (mPlayer.standType == 1)
            {
                Item.type = ModContent.ItemType<RequiemCrownShort>();
                Item.SetDefaults(ModContent.ItemType<RequiemCrownShort>());
            }
        }
    }
}