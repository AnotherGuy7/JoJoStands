using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ViralCupcake : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A well-made cupcake on a tiny plate, all with a tiny viral structure on top!\nBoosts Stand Damage and Stand Crit Chances for 2m.");
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
            player.AddBuff(ModContent.BuffType<StrongWill>(), (2 * 60) * 60);
            player.AddBuff(ModContent.BuffType<SharpMind>(), (2 * 60) * 60);
        }

        public override void AddRecipes()
        {
            CreateRecipe(6)
                .AddIngredient(ModContent.ItemType<ViralPowder>(), 4)
                .AddIngredient(ItemID.HoneyBlock)
                .AddIngredient(ItemID.Hay)
                .Register();
        }
    }
}
