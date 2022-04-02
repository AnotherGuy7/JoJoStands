using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ViralChickenLeg : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A dry, breaded (chicken?) leg that, when eaten, makes you feel much better.\nBoosts Stand Speed and player defense for 3m.");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.useTime = 20;
            item.useAnimation = 20;
            item.value = Item.buyPrice(0, 0, 20, 0);
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
            player.AddBuff(mod.BuffType("QuickThinking"), (3 * 60) * 60);
            player.AddBuff(mod.BuffType("MentalFortitude"), (3 * 60) * 60);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ViralPowder"), 3);
            recipe.AddIngredient(ItemID.Bunny);
            recipe.AddIngredient(ItemID.Bone, 2);
            recipe.AddTile(TileID.Campfire);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
