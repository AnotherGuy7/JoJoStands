using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Body)]
    public class KoichiShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Koichi's Shirt");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 22;
            Item.rare = ItemRarityID.LightPurple;
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