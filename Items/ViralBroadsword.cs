using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class ViralBroadsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A shiny sword that gives you the might to swing effortlessly...");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 15;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.maxStack = 1;
            Item.noUseGraphic = true;
            Item.knockBack = 5f;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ModContent.ProjectileType<ViralBroadswordProjectile>();
            Item.value = Item.buyPrice(0, 1, 25, 0);
            Item.useTurn = true;
            Item.noWet = true;
            Item.channel = true;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Melee;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = player.Center;
            if (Main.MouseWorld.X > player.Center.X)
                player.direction = 1;
            else
                player.direction = -1;

            velocity.X = 1f * player.direction;
            velocity.Y = 0f;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
