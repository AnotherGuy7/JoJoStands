using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StickyFingersT3 : StandItemClass
    {
        public override int standSpeed => 10;
        public override int standType => 1;
        public override string standProjectileName => "StickyFingers";
        public override int standTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/StickyFingersT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sticky Fingers (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and zip them open! Right-click to use an extended punch!\nSpecial: Zip in the direction of your mouse for the distance of 30 tiles!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
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
                .AddIngredient(ModContent.ItemType<StickyFingersT2>())
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.Sapphire, 5)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 2)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
