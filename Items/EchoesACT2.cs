using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class EchoesACT2 : StandItemClass
    {
        public override int standSpeed => 14;
        public override int standType => 1;
        public override string standProjectileName => "Echoes";
        public override int standTier => 3;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Echoes (ACT 2)");
            Tooltip.SetDefault("Left-click to punch enemies at a really fast rate! \nRight-click: Sound effect selection! \nHold right-click: Create the chosen sound effect at the mouse point! \nSpecial: Remote Control \nSecond Special: Switch to previous act!");
        }

        public override void SetDefaults()
        {
            Item.damage = 44;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<EchoesACT1>())
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddIngredient(ItemID.MusicBox)
                .AddIngredient(ItemID.Emerald, 2)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 4)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
