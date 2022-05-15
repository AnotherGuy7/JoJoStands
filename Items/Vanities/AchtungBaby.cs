using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class AchtungBaby : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Makes you invisible!");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
            /*ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
            ArmorIDs.Legs.Sets.HidesTopSkin[Item.legSlot] = true;
            ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = 8;
            Item.vanity = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.invis = true;
            player.GetModPlayer<MyPlayer>().hideAllPlayerLayers = true;
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