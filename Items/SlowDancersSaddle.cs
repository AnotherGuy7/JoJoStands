
using JoJoStands.Mounts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SlowDancersSaddle : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Slow Dancer's Saddle");
            // Tooltip.SetDefault("A blue saddle that belongs to a fast race horse...");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 22;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Pink;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<SlowDancerMount>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddIngredient(ItemID.SoulofMight, 5)
                .Register();
        }
    }
}