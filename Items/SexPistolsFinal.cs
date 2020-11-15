using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SexPistolsFinal : StandItemClass
    {
        public override string Texture
        {
            get { return mod.Name + "/Items/SexPistolsT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sex Pistols (Final)");
            Tooltip.SetDefault("Use a gun and have its bullets home! Increases bullet damages by 20%\nRight-Click to have controlled bullets go in the direction of the mouse.\nUsed in Stand Slot");
        }
        public override void SetDefaults()
        {
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

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SexPistolsT3"));
            recipe.AddIngredient(ItemID.Ectoplasm, 15);
            recipe.AddIngredient(ItemID.LargeTopaz);
            recipe.AddIngredient(ItemID.FallenStar, 7);
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 2);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 4);
            recipe.AddIngredient(ItemID.ChlorophyteBullet, 100);
            recipe.AddIngredient(mod.ItemType("DeterminedLifeforce"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}