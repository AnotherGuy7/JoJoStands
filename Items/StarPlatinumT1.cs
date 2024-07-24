using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StarPlatinumT1 : StandItemClass
    {
        public override int StandSpeed => 11;
        public override int StandType => 1;
        public override string StandIdentifierName => "StarPlatinum";
        public override int StandTier => 1;
        public override Color StandTierDisplayColor => Color.LightPink;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Star Platinum (Tier 1)");
            // Tooltip.SetDefault("Punch enemies at a really fast rate.\nUsed in Stand Slot");
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<TheWorldT1>();
        }

        public override void SetDefaults()
        {
            Item.damage = 23;
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
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
