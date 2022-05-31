using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Body)]
    public class NaranciaShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Narancia's Vest");
            Tooltip.SetDefault("A dark, slim-fitting vest with a collar and straps. Perfect for flexing your arms like a runway.");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
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