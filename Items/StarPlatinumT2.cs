using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StarPlatinumT2 : StandItemClass
    {
        public override int standSpeed => 8;
        public override int standType => 1;
        public override string standProjectileName => "StarPlatinum";
        public override int standTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/StarPlatinumT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Platinum (Tier 2)");
            Tooltip.SetDefault("Left-click to punch enemies at a really fast rate and right-click to use Star Finger!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StarPlatinumT1>())
                .AddIngredient(ItemID.PlatinumBar, 12)
                .AddIngredient(ItemID.Hellstone, 20)
                .AddIngredient(ItemID.Amethyst, 2)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StarPlatinumT1>())
                .AddIngredient(ItemID.GoldBar, 12)
                .AddIngredient(ItemID.Hellstone, 20)
                .AddIngredient(ItemID.Amethyst, 2)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
