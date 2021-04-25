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
			item.damage = 9;
			item.width = 30;
			item.height = 30;        //hitbox's width and height when the item is in the world
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.Stabbing;
			item.maxStack = 1;
			item.knockBack = 1f;
			item.rare = ItemRarityID.LightPurple;
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

            if (player.altFunctionUse == 2 && player.ownedProjectileCounts[mod.ProjectileType("CutterHamonBubble")] <= 0 && hamonPlayer.amountOfHamon < 3)
                return false;

            if (player.altFunctionUse != 2 && hamonPlayer.amountOfHamon < 1)
                return false;

            if (player.altFunctionUse == 2)
            {
                item.damage = 15;
                item.knockBack = 8f;
                item.shoot = mod.ProjectileType("CutterHamonBubble");
            }
            if (player.altFunctionUse != 2)
            {
                item.damage = 9;
                item.knockBack = 1f;
                item.shoot = mod.ProjectileType("HamonBubble");
            }
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (player.altFunctionUse == 2 && hamonPlayer.amountOfHamon >= 3)
            {
                damage += 6;
                type = mod.ProjectileType("CutterHamonBubble");
                knockBack = 8f;
            }
            if (player.altFunctionUse != 2 && hamonPlayer.amountOfHamon > 1)
            {
                if (player.statLife <= (player.statLifeMax * 0.05f))
                {
                    damage += 24;
                    type = mod.ProjectileType("HamonBloodBubble");
                    hamonPlayer.amountOfHamon -= 1;
                }
                else
                {
                    type = mod.ProjectileType("HamonBubble");
                    hamonPlayer.amountOfHamon -= 1;
                }
            }
            return true;
        }

        public override void HoldItem(Player player)
        {
            ChargeHamon();
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
