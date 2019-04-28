using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheWorldT3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The World (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right click to throw knives! \nSpecial: Stop time for 3 seconds! \nNext Tier: 26 Hallowed Bars, 7 Souls of Time");
        }
        public override void SetDefaults()
        {
            item.damage = 91;  
            item.width = 100;
            item.height = 8;
            item.useTime = 11;
            item.useAnimation = 11;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2;
            item.value = 10000;
            item.rare = 6;
            item.melee = true;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("TheWorldFist");
            item.shootSpeed = 50f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float numberProjectiles = 3 + Main.rand.Next(5);
            float rotation = MathHelper.ToRadians(45);

            if (player.altFunctionUse == 2)
            {
                float numberKnives = 3;
                position += Vector2.Normalize(new Vector2(speedX, speedY));
                for (int i = 0; i < numberKnives; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberKnives - 1))) * .2f;
                    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                }
                return true;
            }

            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void HoldItem(Player player)
        {
            if (JoJoStands.ItemHotKey.JustPressed && !player.HasBuff(mod.BuffType("TheWorldCoolDown")))
            {
                player.AddBuff(mod.BuffType("TheWorldBuff"), 180, true);
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.damage = 89;
                item.ranged = true;
                item.width = 100;
                item.height = 8;
                item.useTime = 10;
                item.useAnimation = 10;
                item.useStyle = 5;
                item.knockBack = 2;
                item.autoReuse = false;
                item.shoot = mod.ProjectileType("Knife");
                item.shootSpeed = 55f;
            }
            else
            {
                item.damage = 91;
                item.ranged = true;
                item.width = 10;
                item.height = 8;
                item.useTime = 11;
                item.useAnimation = 11;
                item.useStyle = 5;
                item.knockBack = 2;
                item.autoReuse = true;
                item.shoot = mod.ProjectileType("TheWorldFist");
                item.shootSpeed = 50f;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StarPlatinumT2"));
            recipe.AddIngredient(ItemID.Hellstone, 25);
            recipe.AddIngredient(ItemID.GoldenClock, 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
