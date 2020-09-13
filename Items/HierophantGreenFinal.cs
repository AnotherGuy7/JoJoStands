using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HierophantGreenFinal : StandItemClass
    {
        public override int standSpeed => 15;
        public override int standType => 2;

        public override string Texture
        {
            get { return mod.Name + "/Items/HierophantGreenT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hierophant Green (Final)");
            Tooltip.SetDefault("Shoot emeralds at the enemies and right click to shoot more accurate emralds!\nSpecial: 20 Meter Emerald Splash!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 72;
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
            recipe.AddIngredient(mod.ItemType("HierophantGreenT3"));
            recipe.AddIngredient(ItemID.LargeEmerald, 1);
            recipe.AddIngredient(ItemID.ChlorophyteOre, 12);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 3);
            recipe.AddIngredient(mod.ItemType("WillToChange"), 3);
            recipe.AddIngredient(mod.ItemType("RighteousLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
