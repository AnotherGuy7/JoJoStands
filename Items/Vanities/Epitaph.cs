using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class Epitaph : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Wearing this makes you want to use a frog as a phone");

            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = 8;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.wearingEpitaph = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 2)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}