using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StarPlatinumFinal : StandItemClass
    {
        public override int standSpeed => 6;
        public override int standType => 1;
        public override string standProjectileName => "StarPlatinum";
        public override int standTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/StarPlatinumT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Platinum (Final Tier)");
            Tooltip.SetDefault("Left-click to punch enemies at a really fast rate and right-click to flick a bullet!\nIf there are no bullets in your inventory, Star Finger will be used instead.\nSpecial: Stop time for 4 seconds!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 118;
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
                .AddIngredient(ModContent.ItemType<StarPlatinumT3>())
                .AddIngredient(ItemID.Ectoplasm, 8)
                .AddIngredient(ItemID.LargeAmethyst, 1)
                .AddIngredient(ItemID.FallenStar, 7)
                .AddIngredient(ModContent.ItemType<RighteousLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
