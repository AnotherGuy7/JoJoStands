using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Items.Hamon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class AjaStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Stone of Aja");
            Tooltip.SetDefault("Wear this stone to walk on water, increase your regenerative capabilities, and increase your max hamon!");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 60;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(gold: 14, silver: 50);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
            Item.scale = 0.8f;
        }

        public override void HoldItem(Player player)
        {
            Lighting.AddLight(player.Center, 1f, 0f, 0f);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            hamonPlayer.hamonDamageBoosts += 0.5f;
            hamonPlayer.hamonKnockbackBoosts += 0.5f;
            player.GetDamage(DamageClass.Generic) += 0.2f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.15f;

            player.waterWalk = true;
            player.waterWalk2 = true;
            player.lifeRegenCount += 8;
            hamonPlayer.hamonIncreaseBonus += 2;
            Lighting.AddLight(player.Center, 1f, 0f, 0f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LargeRuby, 1)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 12)
                .AddIngredient(ItemID.SunStone, 1)
                .Register();
        }
    }
}
