using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ViralSteak : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("An already warm, juicy, and mediun-rare steak.\nGrants Stand related buffs upon consumption.");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.useTime = 20;
            item.useAnimation = 20;
            item.value = Item.buyPrice(0, 0, 15, 25);
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
            player.AddBuff(mod.BuffType("CoordinatedEyes"), 90 * 60);
            player.AddBuff(mod.BuffType("StrongWill"), 90 * 60);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ViralPowder"), 4);
            recipe.AddIngredient(ItemID.Bunny, 2);
            recipe.AddIngredient(ItemID.Squirrel, 2);
            recipe.AddTile(TileID.Campfire);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
