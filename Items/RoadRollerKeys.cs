using JoJoStands.Mounts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class RoadRollerKeys : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Road Roller Keys");
            Tooltip.SetDefault("Keys for a Road Roller!");
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
            Item.noUseGraphic = true;
            Item.mountType = ModContent.MountType<RoadRollerMount>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("JoJoStandsIron-TierBar", 20)
                .AddRecipeGroup("JoJoStandsCobalt-TierBar", 13)
                .AddIngredient(ItemID.SoulofNight)
                .Register();
        }
    }
}