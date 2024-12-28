using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class JolyneHair : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jolyne Hair");
            // Tooltip.SetDefault("A wig with layered buns and braids suited to those who fight for their freedoms.");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
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