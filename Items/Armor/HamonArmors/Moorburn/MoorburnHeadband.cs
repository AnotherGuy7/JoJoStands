using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.HamonArmors.Moorburn
{
    [AutoloadEquip(EquipType.Head)]
    public class MoorburnHeadband : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moorburn Headband");
            Tooltip.SetDefault("A headband purified by Hamon.\nIncreases Hamon Damage by 12%");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 10;
            Item.value = Item.buyPrice(0, 0, 55, 50);
            Item.rare = ItemRarityID.Green;
            Item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<Hamon.HamonPlayer>().hamonDamageBoosts += 0.12f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<MoorburnChestplate>() && legs.type == ModContent.ItemType<MoorburnGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+5% Movement Speed\nWhile holding a Hamon Item: Defense is increased by 5.";
            player.moveSpeed *= 1.05f;
            if (player.HeldItem.ModItem is Hamon.HamonDamageClass)
                player.statDefense += 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 6)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 6)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 6)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}