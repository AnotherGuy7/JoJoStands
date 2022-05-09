using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SteelBalls : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Steel Balls");
            Tooltip.SetDefault("These steel balls have been passed down from generation to generation...\nRequires 5 hamon to throw effectively.");
        }
        public override void SetDefaults()
        {
            Item.damage = 74;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = 3;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.knockBack = 2f;
            Item.value = 7;
            Item.rare = 6;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SteelBallP>();
            Item.shootSpeed = 16f;
            Item.useTurn = true;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Hamon.HamonPlayer>();
            if (hamonPlayer.amountOfHamon >= 5)
                damage.Flat += 33f;
        }

        public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback)
        {
            Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Hamon.HamonPlayer>();
            if (hamonPlayer.amountOfHamon >= 5)
                knockback += 1f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Hamon.HamonPlayer>();
            if (hamonPlayer.amountOfHamon >= 5)
            {
                velocity *= 2f;
                hamonPlayer.amountOfHamon -= 5;
            }
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SteelBallP>()] >= 2)
                return false;

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 2)
                .AddIngredient(ItemID.HallowedBar, 7)
                .Register();
        }
    }
}
