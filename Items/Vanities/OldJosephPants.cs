using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Legs)]
    public class OldJosephPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Old Man's Pants");
            Tooltip.SetDefault("Dirty brown pants. At least he shoes look nice!");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
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