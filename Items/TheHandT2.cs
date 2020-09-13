using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheHandT2 : StandItemClass
    {
        public override int standSpeed => 12;
        public override int standType => 1;

        public override string Texture
        {
            get { return mod.Name + "/Items/TheHandT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Hand (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to scrape away space!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 34;
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
            recipe.AddIngredient(mod.ItemType("TheHandT1"));
            recipe.AddIngredient(ItemID.HellstoneBar, 13);
            recipe.AddIngredient(mod.ItemType("WillToDestroy"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
