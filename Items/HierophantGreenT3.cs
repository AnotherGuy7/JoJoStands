using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HierophantGreenT3 : StandItemClass
    {
        public override int StandSpeed => 20;
        public override int StandType => 2;
        public override string StandIdentifierName => "HierophantGreen";
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => Color.LawnGreen;

        public override string Texture
        {
            get { return Mod.Name + "/Items/HierophantGreenT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hierophant Green (Tier 3)");
            /* Tooltip.SetDefault("Left-click to release a flurry of emeralds and right-click to throw a binding emerald string!\nSpecial: 20 Meter Emerald Splash!\nSecond Special: Remote Control" +
                "\nWhile in Remote Mode: Left-click to move and right-click to release a flurry of emeralds!\nRemote Mode Special: Set tripwires for your enemies!\nUsed in Stand Slot"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 56;
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
                .AddIngredient(ModContent.ItemType<HierophantGreenT2>())
                .AddIngredient(ItemID.Emerald, 6)
                .AddIngredient(ItemID.HallowedBar, 6)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 2)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
