using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class WhitesnakeT2 : StandItemClass
    {
        public override int standSpeed => 13;
        public override int standType => 2;
        public override string standProjectileName => "Whitesnake";
        public override int standTier => 2;

        public override string Texture
        {
            get { return mod.Name + "/Items/WhitesnakeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whitesnake (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right click to throw some acid!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 38;
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
            recipe.AddIngredient(mod.ItemType("WhitesnakeT1"));
            recipe.AddIngredient(ItemID.DemoniteBar, 9);
            recipe.AddIngredient(ItemID.Diamond);
            recipe.AddIngredient(ItemID.RottenChunk, 5);
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddIngredient(mod.ItemType("WillToChange"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("WhitesnakeT1"));
            recipe.AddIngredient(ItemID.CrimtaneBar, 9);
            recipe.AddIngredient(ItemID.Diamond);
            recipe.AddIngredient(ItemID.Vertebrae, 5);
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddIngredient(mod.ItemType("WillToChange"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
