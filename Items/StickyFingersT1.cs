using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StickyFingersT1 : StandItemClass
    {
        public override int StandSpeed => 14;
        public override int StandType => 1;
        public override string StandProjectileName => "StickyFingers";
        public override int StandTier => 1;
        public override Color StandTierDisplayColor => StickyFingersFinal.StickyFingersTierColor;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sticky Fingers (Tier 1)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and zip them open!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
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
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
