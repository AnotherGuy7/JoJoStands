using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HermitPurpleFinal : StandItemClass
    {
        public override int StandSpeed => 25;
        public override int StandType => 1;
        public override int StandTier => 4;
        public override string StandIdentifierName => "HermitPurple";
        public override Color StandTierDisplayColor => Color.Magenta;

        public override string Texture
        {
            get { return Mod.Name + "/Items/HermitPurpleT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hermit Purple (Final Tier)");
            // Tooltip.SetDefault("Left-click to use Hermit Purple as a whip and right-click to grab enemies and slowly crush them!\nSpecial: Overcharge Hermit Purple so that enemies that hurt you get blown away!\nPassive: Enemies are hurt when they hurt you and get inflicted Sunburn!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 202;
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
                .AddIngredient(ModContent.ItemType<HermitPurpleT3>())
                .AddIngredient(ItemID.Ectoplasm, 8)
                .AddIngredient(ItemID.TurtleShell, 3)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 8)
                .AddIngredient(ModContent.ItemType<DeterminedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
