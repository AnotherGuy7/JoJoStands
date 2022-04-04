using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class MagiciansRedFinal : StandItemClass
    {
        public override int standSpeed => 14;
        public override int standType => 2;
        public override string standProjectileName => "MagiciansRed";
        public override int standTier => 4;

        public override string Texture
        {
            get { return mod.Name + "/Items/MagiciansRedT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magicians Red (Final Tier)");
            Tooltip.SetDefault("Shoot flaming ankhs at the enemies and right-click to grab an enemy!\nSpecial: Crossfire Hurricane!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 95;
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
            recipe.AddIngredient(mod.ItemType("MagiciansRedT3"));
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddIngredient(ItemID.FireFeather);
            recipe.AddIngredient(mod.ItemType("CaringLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
