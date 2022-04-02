using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors.RustedGladiator
{
    [AutoloadEquip(EquipType.Legs)]
    public class RustedGladiatorBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Boots that have trampled over the lives of many.\n+5% Vampiric Knockback");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = Item.buyPrice(0, 0, 45, 0);
            item.rare = ItemRarityID.Blue;
            item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<VampirePlayer>().vampiricKnockbackMultiplier += 0.05f;
        }
    }
}