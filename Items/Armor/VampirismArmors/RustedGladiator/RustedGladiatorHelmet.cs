using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors.RustedGladiator
{
    [AutoloadEquip(EquipType.Head)]
    public class RustedGladiatorHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A helmet worn by only the best of warriors.\n+5% Vampiric Damaage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 0, 47, 0);
            item.rare = ItemRarityID.Blue;
            item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<VampirePlayer>().vampiricDamageMultiplier += 0.05f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("RustedGladiatorChestplate") && legs.type == mod.ItemType("RustedGladiatorBoots");
        }

        public override void UpdateArmorSet(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            player.setBonus = "Your physical abilities have been boosted.";
            player.moveSpeed += 0.3f;
            player.maxRunSpeed += 0.3f;
            player.meleeSpeed += 0.3f;
            vPlayer.vampiricDamageMultiplier += 0.03f;
            player.jumpBoost = true;
            player.jumpSpeedBoost += 0.2f;
        }
    }
}