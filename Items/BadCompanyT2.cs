using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class BadCompanyT2 : StandItemClass
    {
        public override int StandSpeed => 80;
        public override int StandType => 2;
        public override string StandProjectileName => "BadCompany";
        public override int StandTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/BadCompanyT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bad Company (Tier 2)");
            // Tooltip.SetDefault("Left-click to have your troops shoot toward your mouse and right-click to choose your army!\nUsed in Stand Slot");
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

            mPlayer.badCompanyTier = StandTier;
            mPlayer.maxBadCompanyUnits = 6 * StandTier;
            if (JoJoStands.SoundsLoaded)
                SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SummonCries/Bad Company").WithVolumeScale(JoJoStands.ModSoundsVolume));
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BadCompanyT1>())
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddIngredient(ItemID.MusketBall, 50)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
