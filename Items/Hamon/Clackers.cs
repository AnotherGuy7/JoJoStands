using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
	public class Clackers : HamonDamageClass
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clacker Balls");
			Tooltip.SetDefault("These clackers are now deadly weapons while infused with Hamon \nExperience goes up after each conquer... \nRight-click requires more than 5 hamon\nSpecial: Hamon Breathing");
		}

		public override void SafeSetDefaults()
		{
			item.damage = 16;
			item.width = 28;
			item.height = 32;
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;
            item.noUseGraphic = true;
            item.autoReuse = true;
			item.maxStack = 1;
			item.knockBack = 2f;
			item.rare = 4;
			item.shootSpeed = 8f;
            item.useTurn = true;
            item.noWet = true;
		}

        public override void HoldItem(Player player)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (JoJoStands.SpecialHotKey.Current)
            {
                increaseCounter++;
                player.velocity.X /= 3f;
                hamonPlayer.counter = 0;
                Dust.NewDust(player.position, player.width, player.height, 169, player.velocity.X * -0.5f, player.velocity.Y * -0.5f);
            }
            if (increaseCounter >= 30)
            {
                hamonPlayer.HamonCounter += 1;
                increaseCounter = 0;
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (player.altFunctionUse == 2 && hamonPlayer.HamonCounter >= 5)
            {
                type = mod.ProjectileType("ChargedClackerProjectile");
                hamonPlayer.HamonCounter -= 5;
            }
            return true;
        }

        public override void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit)
        {
            if (target.HasBuff(mod.BuffType("Vampire")))
            {
                target.AddBuff(mod.BuffType("Sunburn"), 120);
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (player.altFunctionUse == 2 && hamonPlayer.HamonCounter >= 5)
            {
                item.damage *= (int)1.5;
                item.useTime = 60;
                item.useAnimation = 60;
                item.useStyle = 5;
                item.knockBack = 2f;
                item.shootSpeed = 12f;
                item.useTurn = true;
                item.noWet = true;
            }
            if (player.altFunctionUse != 2 || (player.altFunctionUse == 2 && hamonPlayer.HamonCounter <= 4))
            {
                item.damage = 16;
                item.useTime = 60;
                item.useAnimation = 60;
                item.useStyle = 5;
                item.knockBack = 2f;
                item.shoot = mod.ProjectileType("ClackerProjectile");
                item.shootSpeed = 8f;
                item.useTurn = true;
                item.noWet = true;
            }
            if ((player.ownedProjectileCounts[mod.ProjectileType("ClackerProjectile")] >= 1) || (player.ownedProjectileCounts[mod.ProjectileType("ChargedClackerProjectile")] >= 1))
            {
                return false;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LeadBar, 30);
            recipe.AddIngredient(ItemID.Cobweb, 25);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 45);
			recipe.SetResult(this);
			recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 30);
            recipe.AddIngredient(ItemID.Cobweb, 25);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 45);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
