using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class MagentMagentHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magent Magent's Cap");
            Tooltip.SetDefault("A magenta colored hat, resembling a magician's. It makes you zone out.");
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