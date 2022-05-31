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
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 0, 47, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<VampirePlayer>().vampiricDamageMultiplier += 0.05f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<RustedGladiatorChestplate>() && legs.type == ModContent.ItemType<RustedGladiatorBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            player.setBonus = "Your physical abilities have been boosted.";
            player.moveSpeed += 0.3f;
            player.maxRunSpeed += 0.3f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.3f;
            vPlayer.vampiricDamageMultiplier += 0.03f;
            player.jumpBoost = true;
            player.jumpSpeedBoost += 0.2f;
        }
    }
}