using System.Collections.Generic;
using JoJoStands.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
	public class Hamon : HamonDamageClass
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hamon");
			Tooltip.SetDefault("Punch enemies with the power that circulates in you. \nExperience goes up after each conquer... \nRight-click requires more than 5 hamon");
		}
		public override void SafeSetDefaults()      //make it like ItemID.3368
		{
			item.damage = 14;
			item.width = 32;
			item.height = 32;        //hitbox's width and height when the item is in the world
			item.maxStack = 1;
            item.noUseGraphic = true;
			item.knockBack = 1f;
			item.rare = 5;
            item.shoot = mod.ProjectileType("HamonPunches");
            item.useTurn = true;
            item.noWet = true;
            item.useAnimation = 25;
            item.useTime = 15;
            item.useStyle = 5;
            item.channel = true;
            item.noMelee = true;
            item.autoReuse = false;
            item.shootSpeed = 15f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2 && player.GetModPlayer<MyPlayer>().HamonCounter >= 5)
            {
                item.noUseGraphic = true;
                item.useTime = 180;
                item.useAnimation = 180;
                item.shoot = -1;
                int healamount = Main.rand.Next(10, 20);
                player.HealEffect(healamount);
                player.statLife += healamount;
                player.GetModPlayer<MyPlayer>().HamonCounter -= 5;
            }
            if (player.altFunctionUse == 2 && player.GetModPlayer<MyPlayer>().HamonCounter <= 5)
            {
                return false;
            }
            if (player.altFunctionUse != 2)
            {
                item.knockBack = 1f;
                item.shoot = mod.ProjectileType("HamonPunches");
                item.useAnimation = 25;
                item.useTime = 15;
                item.channel = true;
                item.autoReuse = false;
                item.shootSpeed = 15f;
            }
            return true;
        }

        public override void HoldItem(Player player)
        {
            player.waterWalk = true;
            player.waterWalk2 = true;
            if (player.statLife <= 25)
            {
                player.AddBuff(mod.BuffType("RUUUN"), 360);
            }
		}

        public override void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit)
        {
            if (target.HasBuff(mod.BuffType("Vampire")))
            {
                target.AddBuff(mod.BuffType("Sunburn"), 180);
            }
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 25);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
