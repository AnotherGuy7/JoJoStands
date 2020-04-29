using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items.Armor
{
    public class LockT3 : ModItem
    {
        public override string Texture
        {
            get { return mod.Name + "/Items/Armor/LockT1"; }
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Lock (Tier 3)");
            Tooltip.SetDefault("Make people that harm you overwhelmed with Guilt! \nSpecial: Damage yourself and make everyone in a 40 block radius guilty about it.");
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
            recipe.AddIngredient(mod.ItemType("StandArrow"));
            recipe.AddIngredient(mod.ItemType("WillToEscape"), 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}