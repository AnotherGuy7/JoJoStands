using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StickyFingersT2 : StandItemClass
    {
        public override int StandSpeed => 13;
        public override int StandType => 1;
        public override string StandProjectileName => "StickyFingers";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => StickyFingersFinal.StickyFingersTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/StickyFingersT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sticky Fingers (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and zip them open! Right-click to use an extended punch!\nHold Right-Click on a tile to hide in it and surprise your enemies!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 37;
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
                .AddIngredient(ModContent.ItemType<StickyFingersT1>())
                .AddIngredient(ItemID.Sapphire, 3)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
