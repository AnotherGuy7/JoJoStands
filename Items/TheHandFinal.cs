using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheHandFinal : StandItemClass
    {
        public override int standSpeed => 10;
        public override int standType => 1;
        public override string standProjectileName => "TheHand";
        public override int standTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/TheHandT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Hand (Final Tier)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to scrape away space!\nSpecial: Switch to Scrape Mode\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 78;
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
                .AddIngredient(ModContent.ItemType<TheHandT3>())
                .AddIngredient(ItemID.ShroomiteBar, 15)
                .AddIngredient(ModContent.ItemType<DeterminedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
