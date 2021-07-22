using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheWorldFinal : StandItemClass
    {
        public override int standSpeed => 8;
        public override int standType => 1;
        public override string standProjectileName => "TheWorld";
        public override int standTier => 4;

        public override string Texture
        {
            get { return mod.Name + "/Items/TheWorldT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The World (Final Tier)");       //To refer to things like "Special during Timestop" where you press special and special again a little later, use "Double-Tap Special"
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to throw knives! \nSpecial: Stop time for 9 seconds!\nSpecial during Timestop: Throw a Road Roller during a timestop!\nSecond Special: Stop time and surround an enemy with knives!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 87;
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
            recipe.AddIngredient(mod.ItemType("TheWorldT3"));
            recipe.AddIngredient(ItemID.Ectoplasm, 15);
            recipe.AddIngredient(mod.ItemType("SoulofTime"), 7);
            recipe.AddIngredient(mod.ItemType("WillToFight"), 3);
            recipe.AddIngredient(mod.ItemType("WillToControl"), 3);
            recipe.AddIngredient(mod.ItemType("TaintedLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
