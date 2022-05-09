using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ViralPunch : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A golden, sour, yet refreshing drink that boosts your mental capabilities somehow.\nBoosts Stand Damage and Stand Speed for 2m.");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = Item.buyPrice(0, 0, 12, 50);
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
            player.AddBuff(ModContent.BuffType<StrongWill>(), 120 * 60);
            player.AddBuff(ModContent.BuffType<QuickThinking>(), 120 * 60);
        }

        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddIngredient(ModContent.ItemType<ViralPowder>(), 3)
                .AddIngredient(ItemID.HoneyBlock, 2)
                .AddIngredient(ItemID.BottledWater, 2)
                .AddIngredient(ItemID.BlueBerries, 3)
                .AddTile(TileID.Kegs)
                .Register();
        }
    }
}
