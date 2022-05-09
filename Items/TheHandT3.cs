using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheHandT3 : StandItemClass
    {
        public override int standSpeed => 11;
        public override int standType => 1;
        public override string standProjectileName => "TheHand";
        public override int standTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/TheHandT1"; }
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Hand (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to scrape away space!\nSpecial: Switch to Scrape Mode\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 52;
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
                .AddIngredient(ModContent.ItemType<TheHandT2>())
                .AddIngredient(ItemID.TitaniumBar, 18)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TheHandT2>())
                .AddIngredient(ItemID.AdamantiteBar, 18)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
