using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
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
            Item.width = 32;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 0, 60, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<VampirePlayer>().lifeStealMultiplier += 0.04f;
        }
    }
}