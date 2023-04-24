using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GratefulDeadT3 : StandItemClass
    {
        public override int StandSpeed => 12;
        public override int StandType => 1;
        public override string StandProjectileName => "GratefulDead";
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => GratefulDeadFinal.GratefulDeadTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/GratefulDeadT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Grateful Dead (Tier 3)");
            // Tooltip.SetDefault("Punch enemies to make them age and right-click to grab them!\nSpecial: Spread Gas\nMore effective on hot biomes.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 67;
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
                .AddIngredient(ModContent.ItemType<GratefulDeadT2>())
                .AddRecipeGroup("JoJoStandsCursedIchor", 10)
                .AddIngredient(ItemID.Bone, 15)
                .AddIngredient(ModContent.ItemType<WillToControl>(), 2)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
