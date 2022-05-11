using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class Knife : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hunter's Knife");
            Tooltip.SetDefault("A sharp knife that is best suited to be thrown.");
        }
        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.width = 9;
            Item.height = 29;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.maxStack = 999;
            Item.knockBack = 1f;
            Item.value = Item.buyPrice(copper: 75);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.KnifeProjectile>();
            Item.shootSpeed = 25f;
        }

        public override bool? UseItem(Player player)
        {
            player.ConsumeItem(ModContent.ItemType<Knife>());
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(75)
                .AddIngredient(ItemID.IronBar, 3)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(75)
                .AddIngredient(ItemID.LeadBar, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
