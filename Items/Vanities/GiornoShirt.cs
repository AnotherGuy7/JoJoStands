using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Body)]
    public class GiornoShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mafioso Suit");
            Tooltip.SetDefault("A pink suit with a large, heart-shaped chest window.");
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
                .AddIngredient(ItemID.Silk, 10)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}