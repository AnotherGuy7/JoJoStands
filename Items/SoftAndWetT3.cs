using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SoftAndWetT3 : StandItemClass
    {
        public override int StandSpeed => 11;
        public override int StandType => 1;
        public override string StandProjectileName => "SoftAndWet";
        public override int StandTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/SoftAndWetT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soft and Wet (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a fast rate and right-click to create a Plunder Bubble!\nPassive: Bubble Generation\nSecond Special: Bubble Barrier!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 63;
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
                .AddIngredient(ModContent.ItemType<SoftAndWetT2>())
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddIngredient(ItemID.WaterBucket)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
