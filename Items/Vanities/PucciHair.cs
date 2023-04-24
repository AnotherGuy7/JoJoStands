using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class PucciHair : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Priest Hairdo");
            // Tooltip.SetDefault("The hairstyle of a priest devoted to two entities.");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 12;
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