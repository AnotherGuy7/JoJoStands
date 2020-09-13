using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheHandFinal : StandItemClass
    {
        public override int standSpeed => 10;
        public override int standType => 1;

        public override string Texture
        {
            get { return mod.Name + "/Items/TheHandT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Hand (Final Tier)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to scrape away space!\nSpecial: Tap special to bring enemies to you and hold special to charge a scrape attack!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 78;
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
            recipe.AddIngredient(mod.ItemType("TheHandT3"));
            recipe.AddIngredient(ItemID.ShroomiteBar, 15);
            recipe.AddIngredient(mod.ItemType("DetermiendLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
