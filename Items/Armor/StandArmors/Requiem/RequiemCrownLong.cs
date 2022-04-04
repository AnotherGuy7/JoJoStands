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
            DisplayName.SetDefault("Requiem Crown (Long-Ranged)");
            Tooltip.SetDefault("A crown made from the finest materials space has offered so far.\n+18% Stand Crit Chance\n+22% Stand Damage\n+1 Stand Speed");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Red;
            item.defense = 15;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("RequiemChestplate") && legs.type == mod.ItemType("RequiemGreaves");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Viral Beetles float around you, defending you from anything that comes your way.";
            player.AddBuff(mod.BuffType("ViralBeetleBuff"), 2);
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standCritChangeBoosts += 18f;
            mPlayer.standDamageBoosts += 0.22f;
            mPlayer.standSpeedBoosts += 1;

            if (mPlayer.standType == 0)
            {
                item.type = mod.ItemType("RequiemCrownNeutral");
                item.SetDefaults(mod.ItemType("RequiemCrownNeutral"));
            }
            if (mPlayer.standType == 1)
            {
                item.type = mod.ItemType("RequiemCrownShort");
                item.SetDefaults(mod.ItemType("RequiemCrownShort"));
            }
        }
    }
}