using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Legs)]
    public class VanillaIceBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Vampiric Acolyte Boots");
            // Tooltip.SetDefault("Made to mercilessly kick the enemies of Lord DIO!");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;
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