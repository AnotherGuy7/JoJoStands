using JoJoStands.Buffs.ItemBuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class IcedCup : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iced Water");
            Tooltip.SetDefault("Water with ice in it, perfect to stay hydrated and cool.");
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddBuff(ModContent.BuffType<CooledOut>(), 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ModContent.ItemType<IceCubes>(), 10)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
