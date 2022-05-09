using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors.IronSlop
{
    [AutoloadEquip(EquipType.Legs)]
    public class IronSlopShorts : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("War shorts made with iron? Allows you to feel the breeze!");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 14;
            Item.value = Item.buyPrice(silver: 6);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 12)
                .AddIngredient(ItemID.MudBlock, 15)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 12)
                .AddIngredient(ItemID.MudBlock, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}