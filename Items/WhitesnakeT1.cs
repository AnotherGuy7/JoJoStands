using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class WhitesnakeT1 : StandItemClass
    {
        public override int StandSpeed => 14;
        public override int StandType => 2;
        public override string StandProjectileName => "Whitesnake";
        public override int StandTier => 1;
        public override Color StandTierDisplayColor => WhitesnakeFinal.WhitesnakeTierColor;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whitesnake (Tier 1)");
            Tooltip.SetDefault("Punch enemies at a really fast rate!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 19;
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
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
