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
			Tooltip.SetDefault("Punch enemies with the power that circulates in you. \nExperience goes up after each conquer... \nRight-click requires more than 5 hamon\nSpecial: Hamon Breathing");
		}
		public override void SafeSetDefaults()
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
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (!mPlayer.StandOut || (mPlayer.StandOut && mPlayer.StandAutoMode))
            {
                if (player.altFunctionUse == 2 && hamonPlayer.HamonCounter >= 5)
                {
                    item.noUseGraphic = true;
                    item.useTime = 180;
                    item.useAnimation = 180;
                    item.shoot = 0;
                    int healamount = Main.rand.Next(10, 20);
                    player.HealEffect(healamount);
                    player.statLife += healamount;
                    hamonPlayer.HamonCounter -= 5;
                }
                if (player.altFunctionUse == 2 && hamonPlayer.HamonCounter <= 5)
                {
                    return false;
                }
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

        private int increaseCounter = 0;

        public override void HoldItem(Player player)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            player.waterWalk = true;
            player.waterWalk2 = true;
            if (player.statLife <= 25)
            {
                player.AddBuff(mod.BuffType("RUUUN"), 360);
            }
            if (JoJoStands.SpecialHotKey.Current)
            {
                increaseCounter++;
                player.velocity.X /= 3f;
                hamonPlayer.hamonIncreaseCounter = 0;
                Dust.NewDust(player.position, player.width, player.height, 169, player.velocity.X * -0.5f, player.velocity.Y * -0.5f);
            }
            if (increaseCounter >= 30)
            {
                hamonPlayer.HamonCounter += 1;
                increaseCounter = 0;
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
