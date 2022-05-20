using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class BadCompanyT2 : StandItemClass
    {
        public override int standSpeed => 80;
        public override int standType => 2;
        public override string standProjectileName => "BadCompany";
        public override int standTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/BadCompanyT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bad Company (Tier 2)");
            Tooltip.SetDefault("Left-click to have your troops shoot toward your mouse and right-click to choose your army!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
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
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BadCompanyT1>())
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddIngredient(ItemID.MusketBall)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
