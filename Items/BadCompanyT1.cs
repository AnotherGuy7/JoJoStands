using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class BadCompanyT1 : StandItemClass
    {
        public override int StandSpeed => 90;
        public override int StandType => 2;
        public override string StandProjectileName => "BadCompany";
        public override int StandTier => 1;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bad Company (Tier 1)");
            // Tooltip.SetDefault("Left-click to have your troops shoot toward your mouse and right-click to choose your army!\nUsed in Stand Slot");
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

            mPlayer.standTier = StandTier;
            mPlayer.maxBadCompanyUnits = 6 * StandTier;
            if (JoJoStands.SoundsLoaded)
                SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SummonCries/Bad Company").WithVolumeScale(JoJoStands.ModSoundsVolume), player.Center);
            return true;
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
