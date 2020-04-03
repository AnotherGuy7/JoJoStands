using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheWorldT3 : ModItem
    {
        public override string Texture
        {
            get { return mod.Name + "/Items/TheWorldT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The World (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right click to throw knives! \nSpecial: Stop time for 5 seconds!\nNote: The knives TW throws are made with 1 iron bar at a furnace and are called 'Hunter's Knives'\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 68;
            item.width = 32;
            item.height = 32;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2f;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = 6;
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
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
