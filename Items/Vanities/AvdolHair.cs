using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class AvdolHair : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fortuneteller Wig");
            Tooltip.SetDefault("A wig, styled after a fortuneteller, and Stand user.");

            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = 6;
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