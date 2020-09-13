using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class MagiciansRedT2 : StandItemClass
    {
        public override int standSpeed => 18;
        public override int standType => 2;

        public override string Texture
        {
            get { return mod.Name + "/Items/MagiciansRedT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magicians Red (Tier 2)");
            Tooltip.SetDefault("Shoot flaming ankhs at the enemies and right-click to bind an enemy!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 48;
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
            recipe.AddIngredient(mod.ItemType("MagiciansRedT1"));
            recipe.AddIngredient(ItemID.HellstoneBar, 8);
            recipe.AddIngredient(ItemID.Fireblossom, 3);
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddIngredient(mod.ItemType("WillToProtect"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
