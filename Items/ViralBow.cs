using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class ViralBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A bow that injects Viral Meteroite into its ammo.\nAll arrows shot become Viral Arrows that chase down all enemies.");
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.width = 24;
            Item.height = 24;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.knockBack = 4f;
            Item.ranged = true;
            Item.noUseGraphic = false;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.buyPrice(0, 1, 25, 0);
            Item.shoot = ModContent.ProjectileType<ViralArrow>();
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 8f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<ViralArrow>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
