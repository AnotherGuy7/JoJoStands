using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
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
			item.damage = 74;
			item.width = 32;
			item.height = 32;
			item.useTime = 45;
			item.useAnimation = 45;
			item.useStyle = 3;
            item.noUseGraphic = true;
			item.maxStack = 1;
			item.knockBack = 2f;
			item.value = 7;
			item.rare = 6;
			item.autoReuse = true;
            item.shoot = mod.ProjectileType("SteelBallP");
			item.shootSpeed = 16f;
            item.useTurn = true;
		}

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Hamon.HamonPlayer>();
            if (hamonPlayer.amountOfHamon >= 5)
            {
                flat += 33f;
            }
        }

        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Hamon.HamonPlayer>();
            if (hamonPlayer.amountOfHamon >= 5)
            {
                knockback += 1f;
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Hamon.HamonPlayer>();
            if (hamonPlayer.amountOfHamon >= 5)
            {
                speedX *= 2f;
                speedY *= 2f;
                hamonPlayer.amountOfHamon -= 5;
            }
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("SteelBallP")] >= 2)
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 2);
            recipe.AddIngredient(ItemID.HallowedBar, 7);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
