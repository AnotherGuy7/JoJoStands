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
            if (player.altFunctionUse == 2 && hamonPlayer.amountOfHamon >= 3)
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
                hamonPlayer.amountOfHamon -= 3;
            }
            if (player.altFunctionUse == 2 && hamonPlayer.amountOfHamon <= 3)
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
                hamonPlayer.amountOfHamon -= 1;
            }
            else
            {
                item.shoot = mod.ProjectileType("HamonBubble");
                item.shootSpeed = 30f;
                hamonPlayer.amountOfHamon -= 1;
            }
            return true;
        }

        public override void HoldItem(Player player)
        {
            ChargeHamon();
            player.waterWalk = true;
            player.waterWalk2 = true;
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
