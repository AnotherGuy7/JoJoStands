using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class PolnareffHair : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Polnareff's Hair");
            // Tooltip.SetDefault("Wear this to feel like a masterful swordsman.");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 38;
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