using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class RequiemGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Requiem Greaves");
            Tooltip.SetDefault("Greaves made with the finest metal out there, enchanced with Viral Meteorite.\n+8% Stand Crit Chance\n+14% Movement Speed");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 17, 0, 0);
            item.rare = ItemRarityID.Red;
            item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standCritChangeBoosts += 8f;
            player.moveSpeed *= 1.14f;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 15);
            recipe.AddIngredient(ItemID.FragmentStardust, 5);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 5);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}