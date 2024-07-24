using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GratefulDeadT2 : StandItemClass
    {
        public override int StandSpeed => 13;
        public override int StandType => 1;
        public override string StandIdentifierName => "GratefulDead";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => GratefulDeadFinal.GratefulDeadTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/GratefulDeadT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Grateful Dead (Tier 2)");
            // Tooltip.SetDefault("Punch enemies to make them age and right-click to grab them!\nMore effective on hot biomes.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 41;
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
                .AddIngredient(ModContent.ItemType<GratefulDeadT1>())
                .AddIngredient(ItemID.Bone, 15)
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
