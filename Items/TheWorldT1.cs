using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheWorldT1 : StandItemClass
    {
        public override int StandSpeed => 13;
        public override int StandType => 1;
        public override string StandIdentifierName => "TheWorld";
        public override int StandTier => 1;
        public override Color StandTierDisplayColor => Color.Yellow;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The World (Tier 1)");
            // Tooltip.SetDefault("Punch enemies at a really fast rate!\nUsed in Stand Slot");
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<StarPlatinumT1>();
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
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
