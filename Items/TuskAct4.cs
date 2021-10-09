using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TuskAct4 : StandItemClass
    {
        public override int standSpeed => 15;
        public override int standType => 2;
        public override int standTier => 4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tusk (ACT 4)");
            Tooltip.SetDefault("Use the infinite energy inside you... \nWhile in Manual Mode: Left-click to deliver a flurry of punches with TA4.\nWhile in Auto Mode: Hold left-click to shoot and control a spinning nail and right-click to shoot an infinite spin nail!\nSpecial: Switch to previous acts!");
        }

        public override void SetDefaults()
        {
            item.damage = 305;
            item.width = 32;
            item.height = 32;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2f;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            mPlayer.standType = 2;
            mPlayer.equippedTuskAct = standTier;
            mPlayer.tuskActNumber = 3;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TuskAct3"));
            recipe.AddIngredient(mod.ItemType("RequiemArrow"));
            recipe.AddIngredient(mod.ItemType("DeterminedLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}