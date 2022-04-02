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
            item.width = 10;
            item.height = 10;
            item.useTime = 20;
            item.useAnimation = 20;
            item.value = Item.buyPrice(0, 0, 12, 50);
            item.UseSound = SoundID.Item2;
            item.rare = ItemRarityID.Green;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(mod.BuffType("StrongWill"), 120 * 60);
            player.AddBuff(mod.BuffType("QuickThinking"), 120 * 60);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ViralPowder"), 3);
            recipe.AddIngredient(ItemID.HoneyBlock, 2);
            recipe.AddIngredient(ItemID.BottledWater, 2);
            recipe.AddIngredient(ItemID.BlueBerries, 3);
            recipe.AddTile(TileID.Kegs);
            recipe.SetResult(this, 3);
            recipe.AddRecipe();
        }
    }
}
