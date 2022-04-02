using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HierophantGreenT3 : StandItemClass
    {
        public override int standSpeed => 20;
        public override int standType => 2;
        public override string standProjectileName => "HierophantGreen";
        public override int standTier => 3;

        public override string Texture
        {
            get { return mod.Name + "/Items/HierophantGreenT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hierophant Green (Tier 3)");
            Tooltip.SetDefault("Left-click to release a flurry of emeralds and right-click to throw a binding emerald string!\nSpecial: 20 Meter Emerald Splash!\nSecond Special: Remote Control" +
                "\nWhile in Remote Mode: Left-click to move and right-click to release a flurry of emeralds!\nRemote Mode Special: Set tripwires for your enemies!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 56;
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
            recipe.AddIngredient(mod.ItemType("HierophantGreenT2"));
            recipe.AddIngredient(ItemID.Emerald, 6);
            recipe.AddIngredient(ItemID.OrichalcumBar, 6);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 2);
            recipe.AddIngredient(mod.ItemType("WillToChange"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("HierophantGreenT2"));
            recipe.AddIngredient(ItemID.Emerald, 6);
            recipe.AddIngredient(ItemID.MythrilBar, 6);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 2);
            recipe.AddIngredient(mod.ItemType("WillToChange"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
