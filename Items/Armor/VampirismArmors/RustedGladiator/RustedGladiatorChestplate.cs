using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using JoJoStands.Items.Vampire;

namespace JoJoStands.Items.Armor.VampirismArmors.RustedGladiator
{
    [AutoloadEquip(EquipType.Body)]
    public class RustedGladiatorChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A chestplate scarred by its intense fights throughout the ages.\n+4% Life Steal gains");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 24;
            item.value = Item.buyPrice(0, 0, 60, 0);
            item.rare = ItemRarityID.Blue;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<VampirePlayer>().lifeStealMultiplier += 0.04f;
        }
    }
}