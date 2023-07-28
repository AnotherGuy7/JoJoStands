using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class BadCompanyT3 : StandItemClass
    {
        public override int StandSpeed => 70;
        public override int StandType => 2;
        public override string StandProjectileName => "BadCompany";
        public override int StandTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/BadCompanyT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bad Company (Tier 3)");
            // Tooltip.SetDefault("Left-click to have your troops shoot toward your mouse and right-click to choose your army!\nSpecial: Order an arial bombing on the land around you and call in soldier reinforcements!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 31;
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
                SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SummonCries/Bad Company").WithVolumeScale(JoJoStands.ModSoundsVolume));
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BadCompanyT2>())
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddRecipeGroup("JoJoStandsEvilBullet", 150)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 3)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
