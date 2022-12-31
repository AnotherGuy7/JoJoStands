using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheHandT3 : StandItemClass
    {
        public override int StandSpeed => 12;
        public override int StandType => 1;
        public override string StandProjectileName => "TheHand";
        public override int StandTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/TheHandT1"; }
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Hand (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to scrape away space!\nSpecial: Switch to Scrape Mode\nLeft-click in Scrape Mode to scrape in front of you!\nRight-Click in Scrape Mode to charge up a scrape.\nIf the scrape is charged up for less than 1s, any enemies in the scrape will be scraped toward you.\nIf the scrape is charged for more than 1s, the scrape becomes lethal and charges damage at a rate of 145dmg / 0.5s!\nEnemies in scrape range are marked blue.\nBeing hit while charging a scrape disrupts the scrape.\nUsed in Stand Slot");
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
                .AddRecipeGroup("JoJoStandsAdamantite-TierBar", 18)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
