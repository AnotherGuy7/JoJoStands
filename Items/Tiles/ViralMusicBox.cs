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
            item.width = 26;
            item.height = 16;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 1;
            item.consumable = true;
            item.value = Item.buyPrice(0, 0, 50, 0);
            item.rare = 1;
            item.maxStack = 1;
            item.createTile = mod.TileType("ViralMusicBoxTile");
            item.accessory = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MusicBox);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}