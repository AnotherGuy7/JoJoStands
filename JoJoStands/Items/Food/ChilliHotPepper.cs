using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Food
{
    public class ChilliHotPepper : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A dry, breaded (chicken?) leg that when eaten makes you feel much better.\nGrants Stand related buffs upon consumption.");
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
            player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " couldn't handle the SPICE!!!"), 800, player.direction);
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
