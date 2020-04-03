using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Tiles
{
    public class RemixTable : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Remix Table");
            Tooltip.SetDefault("A plain looking DJ’s table, imbued with the Meteoric virus. Allows you to create and modify Stands.");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 1;
            item.consumable = true;
            item.value = Item.buyPrice(0, 0, 45, 0);
            item.rare = 1;
            item.maxStack = 999;
            item.createTile = mod.TileType("RemixTableTile");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);      //anvil + copper + any will
            recipe.AddIngredient(ItemID.IronBar, 5);
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddIngredient(mod.ItemType("WillToChange"));
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddIngredient(mod.ItemType("WillToDestroy"));
            recipe.AddIngredient(mod.ItemType("WillToEscape"));
            recipe.AddIngredient(mod.ItemType("WillToProtect"));
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LeadBar, 5);
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddIngredient(mod.ItemType("WillToChange"));
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddIngredient(mod.ItemType("WillToDestroy"));
            recipe.AddIngredient(mod.ItemType("WillToEscape"));
            recipe.AddIngredient(mod.ItemType("WillToProtect"));
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}