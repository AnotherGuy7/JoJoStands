using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ViralHamburger : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A regular ol' hamburger... but sprinkled with Viral Powder!\nBoosts Stand Damage, Stand Crit Chance, Stand Speed, and player defense for 5m.");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = Item.buyPrice(0, 0, 60, 0);
            Item.UseSound = SoundID.Item2;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<StrongWill>(), (5 * 60) * 60);
            player.AddBuff(ModContent.BuffType<SharpMind>(), (5 * 60) * 60);
            player.AddBuff(ModContent.BuffType<QuickThinking>(), (5 * 60) * 60);
            player.AddBuff(ModContent.BuffType<MentalFortitude>(), (5 * 60) * 60);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralPowder>(), 5)
                .AddIngredient(ItemID.Hay, 5)
                .AddIngredient(ItemID.Bunny)
                .Register();
        }
    }
}
