using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheWorldT2 : StandItemClass
    {
        public override int standSpeed => 10;
        public override int standType => 1;
        public override string standProjectileName => "TheWorld";
        public override int standTier => 2;

        public override string Texture
        {
            get { return mod.Name + "/Items/TheWorldT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The World (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate! \nSpecial: Stop time for 2 seconds!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 42;
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
            recipe.AddIngredient(mod.ItemType("TheWorldT1"));
            recipe.AddIngredient(ItemID.HellstoneBar, 25);
            recipe.AddIngredient(ItemID.GoldWatch);
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TheWorldT1"));
            recipe.AddIngredient(ItemID.HellstoneBar, 25);
            recipe.AddIngredient(ItemID.PlatinumWatch);
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
