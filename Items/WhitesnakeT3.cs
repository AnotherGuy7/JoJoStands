using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class WhitesnakeT3 : StandItemClass
    {
        public override int standSpeed => 12;
        public override int standType => 2;
        public override string standProjectileName => "Whitesnake";
        public override int standTier => 3;

        public override string Texture
        {
            get { return mod.Name + "/Items/WhitesnakeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whitesnake (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right click to throw some acid!\nSpecial: Take any enemy's discs!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 69;
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
            recipe.AddIngredient(mod.ItemType("WhitesnakeT2"));
            recipe.AddIngredient(ItemID.SoulofNight, 14);
            recipe.AddIngredient(ItemID.SoulofLight, 3);
            recipe.AddIngredient(ItemID.Diamond, 2);
            recipe.AddIngredient(ItemID.CursedFlame, 3);
            recipe.AddIngredient(mod.ItemType("WillToControl"), 2);
            recipe.AddIngredient(mod.ItemType("WillToDestroy"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("WhitesnakeT2"));
            recipe.AddIngredient(ItemID.SoulofNight, 14);
            recipe.AddIngredient(ItemID.SoulofLight, 3);
            recipe.AddIngredient(ItemID.Diamond, 2);
            recipe.AddIngredient(ItemID.Ichor, 3);
            recipe.AddIngredient(mod.ItemType("WillToControl"), 2);
            recipe.AddIngredient(mod.ItemType("WillToDestroy"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
