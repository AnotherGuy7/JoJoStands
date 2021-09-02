using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class WhitesnakeFinal : StandItemClass
    {
        public override int standSpeed => 11;
        public override int standType => 2;
        public override string standProjectileName => "Whitesnake";
        public override int standTier => 4;

        public override string Texture
        {
            get { return mod.Name + "/Items/WhitesnakeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whitesnake (Final Tier)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right click to throw some acid!\nSpecial: Take any enemy's discs!\nSecond Special: Remote Control" +
                "\nWhile in Remote Mode: Left-click to move and right-click shoot enemies with a pistol!\nRemote Mode Special: Create an aura that puts enemies to sleep!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 88;
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
            recipe.AddIngredient(mod.ItemType("WhitesnakeT3"));
            recipe.AddIngredient(ItemID.Ectoplasm, 7);
            recipe.AddIngredient(ItemID.CursedFlame, 5);
            recipe.AddIngredient(ItemID.VialofVenom);
            recipe.AddIngredient(mod.ItemType("TaintedLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("WhitesnakeT3"));
            recipe.AddIngredient(ItemID.Ectoplasm, 7);
            recipe.AddIngredient(ItemID.Ichor, 5);
            recipe.AddIngredient(ItemID.VialofVenom);
            recipe.AddIngredient(mod.ItemType("TaintedLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
