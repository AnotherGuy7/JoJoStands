using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Crystal
{
    [AutoloadEquip(EquipType.Head)]
    public class CrystalHelmetNeutral : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Helmet (Neutral)");
            Tooltip.SetDefault("A helmet made to empower the force of the wills\nStand stat buffs change depending on stand type.");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standType == 1)
            {
                Item.type = ModContent.ItemType<CrystalHelmetShort>();
                Item.SetDefaults(ModContent.ItemType<CrystalHelmetShort>());
            }
            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<CrystalHelmetLong>();
                Item.SetDefaults(ModContent.ItemType<CrystalHelmetLong>());
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrystalShard, 10)
                .AddIngredient(ItemID.Silk, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}