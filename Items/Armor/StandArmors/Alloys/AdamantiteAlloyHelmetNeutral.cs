using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Alloys
{
    [AutoloadEquip(EquipType.Head)]
    public class AdamantiteAlloyHelmetNeutral : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adamantite Alloy Helmet (Neutral)");
            Tooltip.SetDefault("A helmet fused with Viral Meteorite to empower the user.\nStand stat buffs change depending on stand type.");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standType == 1)
            {
                Item.type = ModContent.ItemType<AdamantiteAlloyHelmetShort>();
                Item.SetDefaults(ModContent.ItemType<AdamantiteAlloyHelmetShort>());
            }
            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<AdamantiteAlloyHelmetLong>();
                Item.SetDefaults(ModContent.ItemType<AdamantiteAlloyHelmetLong>());
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AdamantiteBar, 10)
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}