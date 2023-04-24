using JoJoStands.Buffs.AccessoryBuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Requiem
{
    [AutoloadEquip(EquipType.Head)]
    public class RequiemCrownShort : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Requiem Crown (Short-Ranged)");
            // Tooltip.SetDefault("A crown made from the finest materials space has offered so far.\n+25% Stand Damage\n+3 tiles radius\n+10% Incoming Damage Reduction\nMinor Life Regen boost");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.defense = 28;
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
            mPlayer.standDamageBoosts += 0.25f;
            mPlayer.standRangeBoosts += 16f * 3f;       //3 tiles in radius
            player.endurance += 0.1f;
            player.lifeRegen += 4;

            if (mPlayer.standType == 0)
            {
                Item.type = ModContent.ItemType<RequiemCrownNeutral>();
                Item.SetDefaults(ModContent.ItemType<RequiemCrownNeutral>());
            }
            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<RequiemCrownLong>();
                Item.SetDefaults(ModContent.ItemType<RequiemCrownLong>());
            }
        }
    }
}