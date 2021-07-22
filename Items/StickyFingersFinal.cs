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
            get { return mod.Name + "/Items/StickyFingersT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sticky Fingers (Final Tier)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and zip them open! Right-click to use an extended punch!\nSpecial: Zip in the direction of your mouse for a distance of 30 tiles!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 76;
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StickyFingersT3"));
            recipe.AddIngredient(ItemID.Ectoplasm, 4);
            recipe.AddIngredient(mod.ItemType("CaringLifeforce"));
            recipe.AddIngredient(ItemID.LargeSapphire);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
