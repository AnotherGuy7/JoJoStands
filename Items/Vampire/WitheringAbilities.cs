using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using JoJoStands.NPCs;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace JoJoStands.Items.Vampire
{
	public class WitheringAbilities : VampireDamageClass
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Withering Abilities");
			Tooltip.SetDefault("Hold left-click to charge up a punch! Right-click to extend your arm and grab enemies to crush them in place!\nSpecial: Throw up acidic bile! Enemies hit with the bile will be marked for 45s.");
		}

		public override void SafeSetDefaults()
		{
			item.damage = 31;
			item.width = 26;
			item.height = 26;
			item.useTime = 60;
			item.useAnimation = 60;
            item.consumable = false;
            item.noUseGraphic = true;
			item.maxStack = 1;
			item.knockBack = 12f;
			item.value = 0;
			item.rare = ItemRarityID.Orange;
		}

		private int useCool = 0;
		private int punchChargeTimer = 0;

        public override void HoldItem(Player player)
        {
			MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
			VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
			if (player.whoAmI != item.owner || !vPlayer.zombie || (mPlayer.standOut && !mPlayer.standAutoMode))
				return;

			if (useCool > 0)
				useCool--;

			vPlayer.enemyIgnoreItemInUse = true;
            if (Main.mouseLeft && useCool <= 0)
            {
				punchChargeTimer++;
				if (punchChargeTimer > 5 * 60)
                {
					punchChargeTimer = 5 * 60;
                }
				player.velocity.X *= 0.8f;
				player.velocity.Y *= 0.9f;
			}
			if (!Main.mouseLeft && punchChargeTimer > 0 && useCool <= 0)
            {
				int multiplier = punchChargeTimer / 60;
				if (multiplier == 0)
                {
					multiplier = 1;
                }
				player.direction = 1;
				if (Main.MouseWorld.X - player.position.X < 0)
				{
					player.direction = -1;
				}

				Vector2 launchVector = Main.MouseWorld - player.position;
				launchVector.Normalize();
				launchVector *= multiplier * 4f;
				player.velocity += launchVector;
				useCool += item.useTime + (6 * (punchChargeTimer / 30));
				Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("VampiricPunch"), item.damage * multiplier, item.knockBack * multiplier, item.owner);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 1, 1f, 0.2f);
				punchChargeTimer = 0;
			}

			if (Main.mouseRight && player.ownedProjectileCounts[mod.ProjectileType("ExtendedZombieGrab")] <= 0 && useCool <= 0)
            {
				useCool += 2 * 60;
				Vector2 shootVel = Main.MouseWorld - player.Center;
				shootVel.Normalize();
				shootVel *= 10f;
				int proj = Projectile.NewProjectile(player.Center, shootVel, mod.ProjectileType("ExtendedZombieGrab"), item.damage, 0f, player.whoAmI);
				Main.projectile[proj].netUpdate = true;
			}

			bool specialPressed = false;
			if (player.whoAmI == Main.myPlayer)
				specialPressed = JoJoStands.SpecialHotKey.JustPressed;

			if (specialPressed && useCool <= 0)
            {
				Vector2 shootVel = Main.MouseWorld - player.Center;
				if (shootVel == Vector2.Zero)
				{
					shootVel = new Vector2(0f, 1f);
				}
				shootVel.Normalize();
				shootVel *= 8f;
				useCool += (2 * 60) + 30;

				float numberProjectiles = Main.rand.Next(4, 6 + 1);
				float rotation = MathHelper.ToRadians(30);
				for (int i = 0; i < numberProjectiles; i++)
				{
					float randomSpeedOffset = (100f + Main.rand.NextFloat(-15f, 15f + 1f)) / 100f;
					Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
					perturbedSpeed *= randomSpeedOffset;
					int proj = Projectile.NewProjectile(player.Center, perturbedSpeed, mod.ProjectileType("AcidicBile"), (int)(item.damage * 1.5f), 2f, player.whoAmI);
					Main.projectile[proj].netUpdate = true;
				}
				player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " overstepped their boundaries as a Zombie."), 14, player.direction);
			}
        }

		public override void AddRecipes()
		{
			VampiricItemRecipe recipe = new VampiricItemRecipe(mod, item.type);
			recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
