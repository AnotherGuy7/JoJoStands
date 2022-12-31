using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HierophantGreenFinal : StandItemClass
    {
        public override int StandSpeed => 15;
        public override int StandType => 2;
        public override string StandProjectileName => "HierophantGreen";
        public override int StandTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/HierophantGreenT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hierophant Green (Final Tier)");
            Tooltip.SetDefault("Left-click to release a flurry of emeralds and right-click to throw a binding emerald string!\nSpecial: 20 Meter Emerald Splash!\nSecond Special: Remote Control" +
                "\nWhile in Remote Mode: Left-click to move and right-click to release a flurry of emeralds!\nRemote Mode Special: Set tripwires for your enemies!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 72;
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
                .AddIngredient(ModContent.ItemType<HierophantGreenT3>())
                .AddIngredient(ItemID.LargeEmerald, 1)
                .AddIngredient(ItemID.ChlorophyteOre, 12)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 3)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 3)
                .AddIngredient(ModContent.ItemType<RighteousLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
