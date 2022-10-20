using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GratefulDeadT1 : StandItemClass
    {
        public override int standSpeed => 14;
        public override int standType => 1;
        public override string standProjectileName => "GratefulDead";
        public override int standTier => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grateful Dead (Tier 1)");
            Tooltip.SetDefault("Punch enemies and make them age!\nMore effective on hot biomes.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
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
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
