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
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 40;
            Item.rare = ItemRarityID.LightPurple;
            Item.vanity = true;
            Item.scale = 0.8f;
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