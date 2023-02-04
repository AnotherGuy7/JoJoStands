using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StickyFingersT3 : StandItemClass
    {
        public override int StandSpeed => 12;
        public override int StandType => 1;
        public override string StandProjectileName => "StickyFingers";
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => StickyFingersFinal.StickyFingersTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/StickyFingersT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sticky Fingers (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and zip them open! Right-click to use an extended punch!\nHold Right-Click on a tile to hide in it and surprise your enemies!\nSpecial: Zip in the direction of your mouse for a distance of 30 tiles!\nSecond Special: Prepare to dodge an attack!\nUsed in Stand Slot");
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
