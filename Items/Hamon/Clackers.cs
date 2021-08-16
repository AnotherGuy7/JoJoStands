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
			Tooltip.SetDefault("These clackers are now deadly weapons while infused with Hamon.\nRight-click requires more than 5 hamon\nSpecial: Hamon Breathing");
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
            ChargeHamon();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (player.altFunctionUse == 2 && hamonPlayer.amountOfHamon >= 5)
            {
                damage = (int)(damage * 1.5f);
                speedX *= 1.25f;
                speedY *= 1.25f;
                type = mod.ProjectileType("ChargedClackerProjectile");
                hamonPlayer.amountOfHamon -= 5;
            }
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            if ((player.ownedProjectileCounts[mod.ProjectileType("ClackerProjectile")] >= 1) || (player.ownedProjectileCounts[mod.ProjectileType("ChargedClackerProjectile")] >= 1))
            {
                return false;
            }

            if (player.altFunctionUse != 2 || (player.altFunctionUse == 2 && hamonPlayer.amountOfHamon <= 4))
            {
                item.shoot = mod.ProjectileType("ClackerProjectile");
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
