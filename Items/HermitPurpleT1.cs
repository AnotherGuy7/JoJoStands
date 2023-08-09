using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HermitPurpleT1 : StandItemClass
    {
        public override int StandSpeed => 40;
        public override int StandType => 1;
        public override int StandTier => 1;
        public override Color StandTierDisplayColor => Color.Magenta;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hermit Purple (Tier 1)");
            // Tooltip.SetDefault("Left-click to use Hermit Purple as a whip!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 38;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standType = 1;
            mPlayer.standName = "HermitPurple";
            mPlayer.poseSoundName = "HermitPurple";
            if (JoJoStands.SoundsLoaded)
                SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SummonCries/Hermit Purple").WithVolumeScale(JoJoStands.ModSoundsVolume), player.Center);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
