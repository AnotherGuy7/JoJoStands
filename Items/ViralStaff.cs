using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class ViralStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A gold and well designed staff.\nShoots enemy chasing orbs.");
            Item.staff[Item.type] = true;
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.width = 24;
            Item.height = 24;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.mana = 11;
            Item.knockBack = 0f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.buyPrice(0, 1, 25, 0);
            Item.shoot = ModContent.ProjectileType<ViralStaffProjectile>();
            Item.shootSpeed = 32f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 5;
            float rotation = MathHelper.ToRadians(30);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X + Main.rand.NextFloat(-6f, 6f), velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                int projIndex = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, perturbedSpeed, ModContent.ProjectileType<ViralStaffProjectile>(), damage, knockback, player.whoAmI);
                Main.projectile[projIndex].netUpdate = true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
