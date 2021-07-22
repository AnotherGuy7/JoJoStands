using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheWorldT3 : StandItemClass
    {
        public override int standSpeed => 9;
        public override int standType => 1;
        public override string standProjectileName => "TheWorld";
        public override int standTier => 3;

        public override string Texture
        {
            get { return mod.Name + "/Items/TheWorldT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The World (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right click to throw knives! \nSpecial: Stop time for 5 seconds!\nSecond Special: Stop time and surround an enemy with knives!\nNote: The knives TW throws are made with 1 iron bar at a furnace and are called 'Hunter's Knives'\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 70;
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
            recipe.AddIngredient(mod.ItemType("TheWorldT2"));
            recipe.AddIngredient(ItemID.HallowedBar, 19);
            recipe.AddIngredient(ItemID.GoldBar, 15);
            recipe.AddIngredient(mod.ItemType("SoulofTime"), 2);
            recipe.AddIngredient(mod.ItemType("WillToFight"), 2);
            recipe.AddIngredient(mod.ItemType("WillToControl"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TheWorldT2"));
            recipe.AddIngredient(ItemID.HallowedBar, 19);
            recipe.AddIngredient(ItemID.PlatinumBar, 15);
            recipe.AddIngredient(mod.ItemType("SoulofTime"), 2);
            recipe.AddIngredient(mod.ItemType("WillToFight"), 2);
            recipe.AddIngredient(mod.ItemType("WillToControl"), 2);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
