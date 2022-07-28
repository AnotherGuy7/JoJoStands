using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using JoJoStands;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SilverChariotFinal : StandItemClass
    {
        public override int standSpeed => 5;
        public override int standType => 1;
        public override string standProjectileName => "SilverChariot";
        public override int standTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/SilverChariotT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver Chariot (Final Tier)");
            Tooltip.SetDefault("Left-click to stab enemies and right-click to parry enemies and projectiles away!\nSpecial: Take SC's armor off! (All player damage received is doubled!)\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 76;
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
                .AddIngredient(ModContent.ItemType<SilverChariotT3>())
                .AddIngredient(ItemID.ChlorophyteBar, 14)
                .AddIngredient(ItemID.Ectoplasm, 5)
                .AddRecipeGroup("JoJoStandsGold-TierBar", 4)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 2)
                .AddIngredient(ModContent.ItemType<RighteousLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
