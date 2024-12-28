using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Viral
{
    [AutoloadEquip(EquipType.Head)]
    public class ViralArmorHelmetNeutral : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Viral Helmet (Neutral)");
            // Tooltip.SetDefault("A helmet created from a meteor, powered up by a strange virus. \nThe helmet seems to morph depending on your soul...\nStand stat buffs change depending on stand type.");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.value = Item.buyPrice(0, 3, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standType == 1)
            {
                Item.type = ModContent.ItemType<ViralArmorHelmetMelee>();
                Item.SetDefaults(ModContent.ItemType<ViralArmorHelmetMelee>());
            }
            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<ViralArmorHelmetRanged>();
                Item.SetDefaults(ModContent.ItemType<ViralArmorHelmetRanged>());
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 20)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}