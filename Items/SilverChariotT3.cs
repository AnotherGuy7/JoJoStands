using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SilverChariotT3 : StandItemClass
    {
        public override int StandSpeed => 8;
        public override int StandType => 1;
        public override string StandProjectileName => "SilverChariot";
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => Color.LightGray;

        public override string Texture
        {
            get { return Mod.Name + "/Items/SilverChariotT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Silver Chariot (Tier 3)");
            // Tooltip.SetDefault("Left-click to stab enemies and right-click to parry enemies and projectiles away!\nSpecial: Take SC's armor off! (All player damage received is doubled!)\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 51;
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
                .AddIngredient(ModContent.ItemType<SilverChariotT2>())
                .AddIngredient(ItemID.HallowedBar, 14)
                .AddRecipeGroup("JoJoStandsSilver-TierBar", 12)
                .AddRecipeGroup("JoJoStandsGold-TierBar", 2)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
