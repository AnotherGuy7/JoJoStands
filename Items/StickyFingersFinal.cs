using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StickyFingersFinal : StandItemClass
    {
        public override int standSpeed => 9;
        public override int standType => 1;
        public override string standProjectileName => "StickyFingers";
        public override int standTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/StickyFingersT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sticky Fingers (Final Tier)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and zip them open! Right-click to use an extended punch!\nSpecial: Zip in the direction of your mouse for a distance of 30 tiles!\nUsed in Stand Slot");
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
