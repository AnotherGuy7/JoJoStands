using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StarPlatinumT2 : StandItemClass
    {
        public override int StandSpeed => 10;
        public override int StandType => 1;
        public override string StandProjectileName => "StarPlatinum";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => Color.LightPink;

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
            Item.damage = 56;
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
                .AddRecipeGroup("JoJoStandsGold-TierBar", 12)
                .AddIngredient(ItemID.HellstoneBar, 20)
                .AddIngredient(ItemID.Amethyst, 2)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
