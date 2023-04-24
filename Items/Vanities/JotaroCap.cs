using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class JotaroCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jotaro's Cap");
            // Tooltip.SetDefault("Is it hat or hair? Yare Yare Daze...");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
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