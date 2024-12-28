using JoJoStands.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Items.Tiles
{
    public class OfficersRegret : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Officer's Regret");
            // Tooltip.SetDefault("`D. Storm`");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(gold: 2);
            Item.rare = ItemRarityID.Blue;
            Item.createTile = ModContent.TileType<OfficersRegretTile>();
        }
    }
}