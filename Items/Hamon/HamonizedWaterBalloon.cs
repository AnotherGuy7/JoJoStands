using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class HamonizedWaterBalloon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Left-click to throw this Hamon-Infused Balloon and right-click to drop it as a trap!\nRequires 4 or more Hamon to be used.\nSpecial: Hamon Breathing");
        }

        public override void SetDefaults()
        {
            item.ranged = true;
            item.damage = 18;
            item.width = 30;
            item.height = 28;
            item.useTime = 75;
            item.useAnimation = 75;
            item.maxStack = 999;
            item.knockBack = 5f;
            item.rare = ItemRarityID.Pink;
            item.noWet = true;
            item.useTurn = true;
            item.autoReuse = false;
            item.noUseGraphic = true;
            item.consumable = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("HamonizedWaterBalloonProjectile");
            item.shootSpeed = 9f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<HamonPlayer>().amountOfHamon >= 4;
        }

        public override bool UseItem(Player player)
        {
            if (player.whoAmI == item.owner)
            {
                player.ConsumeItem(item.type);
            }
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = new Vector2(speedX, speedY);
            if (player.altFunctionUse == 2)
            {
                velocity *= 0.5f;
                Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI, 1f);
            }
            else
            {
                Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI, 0f);
            }
            player.GetModPlayer<HamonPlayer>().amountOfHamon -= 4;
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SunDroplet"));
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.SetResult(this, 15);
            recipe.AddRecipe();
        }
    }
}
