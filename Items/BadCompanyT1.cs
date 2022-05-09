using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class BadCompanyT1 : StandItemClass
    {
        public override int standSpeed => 90;
        public override int standType => 2;
        public override string standProjectileName => "BadCompany";
        public override int standTier => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bad Company (Tier 1)");
            Tooltip.SetDefault("Left-click to have your troops shoot toward your mouse and right-click to choose your army!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.width = 46;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            mPlayer.badCompanyTier = standTier;
            mPlayer.maxBadCompanyUnits = 6 * standTier;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToDestroy>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
