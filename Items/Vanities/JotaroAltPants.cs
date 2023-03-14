using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Legs)]
    public class JotaroAltPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jotaro's 200K Yen Pants (Alt)");
            Tooltip.SetDefault("A pair of pants with a blue waist piece.");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
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