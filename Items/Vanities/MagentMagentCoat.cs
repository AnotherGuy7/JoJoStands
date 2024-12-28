using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Body)]
    public class MagentMagentCoat : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Magent Magent's Coat");
            // Tooltip.SetDefault("A bizarre magenta suit, resembling a magician's. Has several pockets for tricks and dynamite.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
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