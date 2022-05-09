using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Legs)]
    public class GiornoPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mafioso Pants");
            Tooltip.SetDefault("A surprisingly normal pair of pants, pink in color.");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = 6;
            Item.vanity = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 5)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}