using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HermitPurpleT1 : StandItemClass
    {
        public override int standSpeed => 40;
        public override int standType => 1;
        public override int standTier => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hermit Purple (Tier 1)>();
            Tooltip.SetDefault("Left-click to use Hermit Purple as a whip!\nUsed in Stand Slot>();
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
            mPlayer.hermitPurpleTier = standTier;
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
