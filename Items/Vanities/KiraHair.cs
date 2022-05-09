using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class KiraHair : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kira's Hair");
            Tooltip.SetDefault("Perfectly average blond hair.");
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
                .AddIngredient(ItemID.Silk, 2)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}