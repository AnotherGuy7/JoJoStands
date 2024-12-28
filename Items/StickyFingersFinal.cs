using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StickyFingersFinal : StandItemClass
    {
        public override int StandSpeed => 11;
        public override int StandType => 1;
        public override string StandIdentifierName => "StickyFingers";
        public override int StandTier => 4;
        public static readonly Color StickyFingersTierColor = new Color(248, 210, 22);
        public override Color StandTierDisplayColor => StickyFingersTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/StickyFingersT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sticky Fingers (Final Tier)");
            // Tooltip.SetDefault("Punch enemies at a really fast rate and zip them open! Right-click to use an extended punch!\nHold Right-Click on a tile to hide in it and surprise your enemies!\nSpecial: Zip in the direction of your mouse for a distance of 30 tiles!\nSecond Special: Prepare to dodge an attack!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 76;
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
                .AddIngredient(ModContent.ItemType<StickyFingersT3>())
                .AddIngredient(ItemID.Ectoplasm, 4)
                .AddIngredient(ModContent.ItemType<CaringLifeforce>())
                .AddIngredient(ItemID.LargeSapphire)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
