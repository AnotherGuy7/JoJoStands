using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;

namespace JoJoStands.Items.Armor
{
    public class CenturyBoyT2 : ModItem
    {
        public override string Texture
        {
            get { return mod.Name + "/Items/Armor/CenturyBoy"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("20th Century Boy (Tier 2)");
            Tooltip.SetDefault("Use the special ability key to make yourself immune to damage, but unable to move or use items.\nSpecial + Right-click: Set off an explosion! (Dynamite required)\nUsed in Stand Slot.");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = 6;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CenturyBoy"));
            recipe.AddIngredient(ItemID.CobaltBar, 6);
            recipe.AddIngredient(ItemID.Dynamite, 5);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CenturyBoy"));
            recipe.AddIngredient(ItemID.PalladiumBar, 6);
            recipe.AddIngredient(ItemID.Dynamite, 5);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}