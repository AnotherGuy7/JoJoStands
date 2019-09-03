using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class StickyFingersT2 : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/StickyFingersT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sticky Fingers (Tier 2)");
			Tooltip.SetDefault("Punch enemies at a really fast rate and zip them open!");
		}

		public override void SetDefaults()
		{
			item.damage = 32;	//Around middle of pre-hardmode
			item.width = 100;
			item.height = 8;
			item.useTime = 13;
			item.useAnimation = 13;
			item.useStyle = 5;
			item.maxStack = 1;
			item.knockBack = 2;
			item.value = 10000;
			item.rare = 6;
			item.melee = true;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
			item.shoot = mod.ProjectileType("StickyFingersFist");
			item.shootSpeed = 50f;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            if (player.altFunctionUse == 2)
            {
                return true;
            }
            float numberProjectiles = 3 + Main.rand.Next(5);
			float rotation = MathHelper.ToRadians(45);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.damage = 47;
                item.width = 10;
                item.height = 8;
                item.useTime = 23;
                item.useAnimation = 23;
                item.useStyle = 5;
                item.knockBack = 2;
                item.autoReuse = true;
                item.shoot = mod.ProjectileType("StickyFingersFistExtended");
                item.shootSpeed = 50f;
            }
            else
            {
                item.damage = 32;
                item.width = 10;
                item.height = 8;
                item.useTime = 13;
                item.useAnimation = 13;
                item.useStyle = 5;
                item.knockBack = 2;
                item.autoReuse = true;
                item.shoot = mod.ProjectileType("StickyFingersFist");
                item.shootSpeed = 50f;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StickyFingersT1"));
            recipe.AddIngredient(ItemID.Sapphire, 16);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
