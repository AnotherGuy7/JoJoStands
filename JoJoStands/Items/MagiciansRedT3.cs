using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class MagiciansRedT3 : StandItemClass
    {
        public override int standSpeed => 16;
        public override int standType => 2;
        public override string standProjectileName => "MagiciansRed";
        public override int standTier => 3;

        public override string Texture
        {
            get { return mod.Name + "/Items/MagiciansRedT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magicians Red (Tier 3)");
            Tooltip.SetDefault("Shoot flaming ankhs at the enemies and right-click to grab an enemy!\nSpecial: Crossfire Hurricane!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 74;
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
            recipe.AddIngredient(mod.ItemType("MagiciansRedT2"));
            recipe.AddIngredient(mod.ItemType("WillToEscape"));
            recipe.AddIngredient(mod.ItemType("WillToProtect"));
            recipe.AddIngredient(ItemID.LivingFireBlock, 32);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
