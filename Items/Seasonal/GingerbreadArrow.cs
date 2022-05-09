using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Seasonal
{
    public class GingerbreadArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Stab yourself with this to for a 55% chance to give yourself a Christmas stand!.. or so it seemed?\nEat this arrow to gain a Christmas Stand.");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.maxStack = 1;
            Item.useStyle = 3;
            Item.noUseGraphic = true;
            Item.rare = 8;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
                player.QuickSpawnItem(player.GetSource_FromThis(), Main.rand.Next(JoJoStands.christmasStands.ToArray()));

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ItemID.GingerbreadCookie, 3)
                .AddIngredient(ItemID.Ectoplasm, 2)
                .AddIngredient(ItemID.Present, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}