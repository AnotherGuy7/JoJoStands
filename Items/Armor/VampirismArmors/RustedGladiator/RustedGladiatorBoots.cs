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
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 45, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<VampirePlayer>().vampiricKnockbackMultiplier += 0.05f;
        }
    }
}