using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class GoldExperienceT1 : ModItem
	{

        public override string Texture
        {
            get { return mod.Name + "/Items/GoldExperienceFinal"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Experience (Tier 1)");
			Tooltip.SetDefault("Punch enemies at a fast rate and right-click to create a frog! \nSpecial: Switches the abilities used for right-click!");
        }
		public override void SetDefaults()
		{
            item.damage = 16;
            item.width = 32;
            item.height = 32;
            item.useTime = 13;
            item.useAnimation = 13;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 1f;
            item.value = Item.buyPrice(0,58, 26, 82);
            item.rare = 8;
            item.melee = true;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("GoldExperienceFist");
            item.shootSpeed = 50f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
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
            if (player.altFunctionUse == 2 && !player.HasBuff(mod.BuffType("GEAbilityCooldown")))
            {
                Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("GEFrog"), 1, 0f, Main.myPlayer, 0f, 0f);
                player.AddBuff(mod.BuffType("GEAbilityCooldown"), 360);
            }
            if (player.altFunctionUse == 2)
            {
                item.shoot = 0;
            }
            if (player.altFunctionUse != 2)
            {
                item.useTime = 10;
                item.useAnimation = 10;
                item.useStyle = 5;
                item.autoReuse = true;
                item.shoot = mod.ProjectileType("GoldExperienceFist");
                item.shootSpeed = 50f;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
