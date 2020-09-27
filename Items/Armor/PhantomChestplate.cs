using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class PhantomChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Chestplate");
            Tooltip.SetDefault("A bright, glowing chestplate made with an adaptable virus and ectoplasm.\n+8% Stand Crit Chance\n+1 Stand Speed\n12% Incoming Damage Reduction");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 4, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standCritChangeBoosts += 8f;
            mPlayer.standSpeedBoosts += 1;
            mPlayer.phantomChestplateEquipped = true;
            Lighting.AddLight(player.Center, 0.173f, 0.224f, 0.230f);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Ectoplasm, 20);
            recipe.AddIngredient(mod.ItemType("ViralPearl"));
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 4);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}