using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Viral
{
    [AutoloadEquip(EquipType.Body)]
    public class ViralArmorKaruta : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Karuta");
            Tooltip.SetDefault("A suit of armor made from a mysterious meteoric alloy, powered up by a strange virus.\nProvides a 4% Stand Crit Chance boost");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 4f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 35)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}