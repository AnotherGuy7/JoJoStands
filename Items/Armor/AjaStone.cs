using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor
{
    public class AjaStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Stone of Aja");
            Tooltip.SetDefault("Wear this stone to walk on water and increase your regenerative capabilities.");
        }
        public override void SetDefaults()
        {
            item.width = 100;
            item.height = 8;
            item.maxStack = 1;
            item.value = 10000;
            item.rare = 6;
            item.accessory = true;
        }

        public override void HoldItem(Player player)
        {
            Vector2 position = default(Vector2);
            //add light(red preferable)
            Lighting.AddLight(position, 1f, 1f, 1f);
            base.HoldItem(player);
        }

        public override void UpdateEquip(Player player)
        {
            if (Main.dayTime == true)
            {
                player.meleeDamage += 0.23f;
                player.meleeSpeed += 0.18f;
                player.magicDamage *= 2;
                player.manaRegen *= 3;
                player.lifeRegen += 2;
                player.waterWalk = true;
                player.waterWalk2 = true;
            }
            player.lifeRegen += 1;
            player.waterWalk = true;
            player.waterWalk2 = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LargeRuby, 1);
            recipe.AddIngredient(ItemID.SunStone, 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
