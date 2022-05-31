using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Requiem
{
    [AutoloadEquip(EquipType.Legs)]
    public class RequiemGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Requiem Greaves");
            Tooltip.SetDefault("Greaves made with the finest metal out there, enchanced with Viral Meteorite.\n+8% Stand Crit Chance\n+14% Movement Speed");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 16);
            Item.rare = ItemRarityID.Red;
            Item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standCritChangeBoosts += 8f;
            player.moveSpeed *= 1.14f;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 15)
                .AddIngredient(ItemID.FragmentStardust, 5)
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}