using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Legs)]
    public class NaranciaPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Narancia's Pants");
            Tooltip.SetDefault("Dark, slim-fitting pants paired with an orange skirt. The shoes are surprisingly durable.");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
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