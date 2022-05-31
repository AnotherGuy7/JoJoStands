using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class BucciaratiHair : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bowl-Haired Wig");
            Tooltip.SetDefault("A black wig in a bowl cut style, resembling a certain mafioso.");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
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