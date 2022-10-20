using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KillerQueenT2 : StandItemClass
    {
        public override int standSpeed => 13;
        public override int standType => 1;
        public override string standProjectileName => "KillerQueen";
        public override int standTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/KillerQueenT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Killer Queen (1st Bomb Tier 2)");
            Tooltip.SetDefault("Left-click to punch and right-click to trigger any block! \nRange: 12 blocks\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
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
                .AddIngredient(ModContent.ItemType<KillerQueenT1>())
                .AddIngredient(ItemID.Dynamite, 12)
                .AddIngredient(ItemID.HellstoneBar, 5)
                .AddIngredient(ModContent.ItemType<WillToDestroy>())
                .AddIngredient(ModContent.ItemType<WillToEscape>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}