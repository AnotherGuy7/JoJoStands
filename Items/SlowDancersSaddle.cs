
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
            DisplayName.SetDefault("Slow Dancer's Saddle");
            Tooltip.SetDefault("A blue saddle that belongs to a fast race horse...");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 1;
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