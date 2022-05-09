using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Tiles
{
    public class ViralMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A music box with a tune encrypted into it.");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 16;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 0, 50, 0);
            Item.rare = 1;
            Item.maxStack = 1;
            Item.createTile = ModContent.TileType<ViralMusicBoxTile>();
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MusicBox)
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}