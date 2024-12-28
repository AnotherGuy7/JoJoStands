using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SilverChariotT2 : StandItemClass
    {
        public override int StandSpeed => 9;
        public override int StandType => 1;
        public override string StandIdentifierName => "SilverChariot";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => Color.LightGray;

        public override string Texture
        {
            get { return Mod.Name + "/Items/SilverChariotT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Silver Chariot (Tier 2)");
            // Tooltip.SetDefault("Left-click to stab enemies and right-click to parry enemies and projectiles away!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 34;
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
                .AddIngredient(ModContent.ItemType<SilverChariotT1>())
                .AddRecipeGroup("JoJoStandsGold-TierBar", 9)
                .AddIngredient(ItemID.Hellstone, 16)
                .AddIngredient(ItemID.Amethyst, 2)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
