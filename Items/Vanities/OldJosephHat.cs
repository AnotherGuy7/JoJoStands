using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class OldJosephHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Old Man's Hat");
            Tooltip.SetDefault("A western hat. Comes with a beard!");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
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