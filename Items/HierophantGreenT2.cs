using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HierophantGreenT2 : StandItemClass
    {
        public override int standSpeed => 30;
        public override int standType => 2;
        public override string standProjectileName => "HierophantGreen";
        public override int standTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/HierophantGreenT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hierophant Green (Tier 2)");
            Tooltip.SetDefault("Left-click to release a flurry of emeralds and right-click to throw a binding emerald string!\nSecond Special: Remote Control" +
                "\nWhile in Remote Mode: Left-click to move and right-click to release a flurry of emeralds!\nRemote Mode Special: Set tripwires for your enemies!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 32;
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
                .AddIngredient(ModContent.ItemType<HierophantGreenT1>())
                .AddIngredient(ItemID.Emerald, 7)
                .AddIngredient(ItemID.TissueSample, 4)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HierophantGreenT1>())
                .AddIngredient(ItemID.Emerald, 7)
                .AddIngredient(ItemID.ShadowScale, 4)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
