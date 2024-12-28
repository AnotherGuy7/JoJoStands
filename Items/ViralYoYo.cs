using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class ViralYoYo : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Viral Yoyo");
            // Tooltip.SetDefault("A deadly yoyo with small spikes to cut and infect your enemies.\nInflicts Infected upon enemy hit.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.width = 24;
            Item.height = 24;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.knockBack = 4f;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.buyPrice(0, 1, 25, 0);
            Item.shoot = ModContent.ProjectileType<ViralYoYoProjectile>();
            Item.shootSpeed = 16f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 3)
                .AddIngredient(ItemID.WhiteString)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
