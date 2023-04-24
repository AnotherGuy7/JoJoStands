using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Requiem
{
    [AutoloadEquip(EquipType.Body)]
    public class RequiemChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Requiem Chestplate");
            // Tooltip.SetDefault("A chestplate made with the finest alloy of Luminite and Viral Meteorite.\n+10% Stand Crit Chance\n+3 Stand Speed\n5% Incoming Damage Reduction");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 20);
            Item.rare = ItemRarityID.Red;
            Item.defense = 24;
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
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 20)
                .AddIngredient(ItemID.FragmentStardust, 7)
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}