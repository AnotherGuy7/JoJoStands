using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class KosakuHair : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kira's (Kosaku) Hair");
            Tooltip.SetDefault("A new hairstyle for a man with confidence.");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 16;
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