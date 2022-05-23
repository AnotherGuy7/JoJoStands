using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Items.Hamon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Front)]
    public class BloodiedMonocle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloodied Monocle");
            Tooltip.SetDefault("While undead and worn, enemies whose blood has not been consumed are highlighted.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 28;
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
            if (Main.dayTime)
            {
                player.GetDamage(DamageClass.Melee) += 0.23f;
                player.GetAttackSpeed(DamageClass.Melee) += 0.18f;
                player.GetDamage(DamageClass.Magic) += 0.23f;
                player.manaRegen *= 3;
                player.lifeRegen += 2;
            }

            player.lifeRegen += 1;
            player.waterWalk = true;
            player.waterWalk2 = true;
            Lighting.AddLight(player.Center, 1f, 0f, 0f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldBar, 2)
                .AddIngredient(ItemID.Glass, 2)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBar, 2)
                .AddIngredient(ItemID.Glass, 2)
                .Register();
        }
    }
}
