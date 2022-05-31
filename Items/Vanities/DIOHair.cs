using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class DIOHair : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DIO's Hair");
            Tooltip.SetDefault("Hair fit for the 'Ruler of Humanity', with a headband.");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 20;
            Item.rare = ItemRarityID.LightPurple;
            Item.vanity = true;
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