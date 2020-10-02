using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class RequiemCrownShort : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Requiem Crown (Short-Ranged)");
            Tooltip.SetDefault("A crown made from the finest materials space has offered so far.\n+25% Stand Damage\n+3 tiles radius\n+10% Incoming Damage Reduction\nMinor Life Regen boost");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Red;
            item.defense = 28;
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
            mPlayer.standDamageBoosts += 0.25f;
            mPlayer.standRangeBoosts += 16f * 3f;       //3 tiles in radius
            player.endurance += 0.1f;
            player.lifeRegen += 4;

            if (mPlayer.standType == 0)
            {
                item.type = mod.ItemType("RequiemCrownNeutral");
                item.SetDefaults(mod.ItemType("RequiemCrownNeutral"));
            }
            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("RequiemCrownLong");
                item.SetDefaults(mod.ItemType("RequiemCrownLong"));
            }
        }
    }
}