using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Legs)]
    public class DiavoloPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Diavolo's Pants");
            Tooltip.SetDefault("A pair of stylish yellow pants, with heart-adorned knees as well as tassels from the waist.");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 18;
            Item.rare = ItemRarityID.LightPurple;
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