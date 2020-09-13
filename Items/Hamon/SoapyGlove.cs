using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
	public class SoapyGlove : HamonDamageClass
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soapy Glove");
			Tooltip.SetDefault("Shoot controllable bubbles! \nExperience goes up after each conquer... \nRight-click requires more than 3 hamon\nSpecial: Hamon Breathing");
		}

		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 30;
			item.height = 8;        //hitbox's width and height when the item is in the world
			item.useTime = 8;
			item.useAnimation = 8;
			item.useStyle = 3;
			item.maxStack = 1;
			item.knockBack = 1;
			item.rare = 6;
            item.UseSound = SoundID.Item85;
            item.shoot = mod.ProjectileType("HamonBubble");
			item.shootSpeed = 4f;
            item.useTurn = true;
            item.noWet = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (player.altFunctionUse == 2 && hamonPlayer.HamonCounter >= 3)
            {
                item.damage = 17;
                item.width = 30;
                item.height = 18;
                item.useTime = 12;
                item.useAnimation = 12;
                item.useStyle = 3;
                item.knockBack = 3;
                item.autoReuse = false;
                item.shoot = mod.ProjectileType("CutterHamonBubble");
                item.shootSpeed = 7f;
                hamonPlayer.HamonCounter -= 3;
            }
            if (player.altFunctionUse == 2 && hamonPlayer.HamonCounter <= 3)
            {
                return false;
            }
            if (player.altFunctionUse != 2)
            {
                item.damage = 15;
                item.width = 30;
                item.height = 30;
                item.useTime = 8;
                item.useAnimation = 8;
                item.useStyle = 3;
                item.knockBack = 1;
                item.autoReuse = false;
                item.shoot = mod.ProjectileType("HamonBubble");
                item.shootSpeed = 10f;
            }
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (player.statLife <= 50)
            {
                item.shoot = mod.ProjectileType("HamonBloodBubble");
                item.shootSpeed = 5f;
                item.damage += 24;
                hamonPlayer.HamonCounter -= 1;
            }
            else
            {
                item.shoot = mod.ProjectileType("HamonBubble");
                item.shootSpeed = 30f;
                hamonPlayer.HamonCounter -= 1;
            }
            return true;
        }

        private int increaseCounter = 0;

        public override void HoldItem(Player player)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            //Dust.NewDust(player.position,  player.width, player.height, 169);    the old dust was unwanted
            player.waterWalk = true;
            player.waterWalk2 = true;
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

        public override void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit)
        {
            if (target.HasBuff(mod.BuffType("Vampire")))
            {
                target.AddBuff(mod.BuffType("Sunburn"), 80);
            }
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 14);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
