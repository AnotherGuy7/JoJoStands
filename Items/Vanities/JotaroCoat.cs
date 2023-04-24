using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Body)]
    public class JotaroCoat : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jotaro's Coat");
            // Tooltip.SetDefault("A black school uniform. Very unique.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
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