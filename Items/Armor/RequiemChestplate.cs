using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class RequiemChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Requiem Chestplate");
            Tooltip.SetDefault("A chestplate made with the finest alloy of Luminite and Viral Meteorite.\n+10% Stand Crit Chance\n+3 Stand Speed\n5% Incoming Damage Reduction");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 20, 0, 0);
            item.rare = ItemRarityID.Red;
            item.defense = 24;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standCritChangeBoosts += 10f;
            mPlayer.standSpeedBoosts += 3;
            player.endurance += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarBar, 20);
            recipe.AddIngredient(ItemID.FragmentStardust, 7);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 8);
            recipe.AddIngredient(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}