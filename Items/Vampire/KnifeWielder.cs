using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace JoJoStands.Items.Vampire
{
	public class KnifeWielder : VampireDamageClass
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Knife Wielder Abilities (Zombie)");
			Tooltip.SetDefault("Left-click to lunge at enemies with knives and right-click to bury knives into you.\nSpecial: Shoot all of the knives inside of you outward!\nNote: 16 or more knives are required for Knife Amalgamation. Knife Amalgamation is required to use the Special.");
		}

		public override void SafeSetDefaults()
		{
			item.damage = 19;
			item.width = 26;
			item.height = 28;
			item.useTime = 30;
			item.useAnimation = 30;
            item.consumable = false;
            item.noUseGraphic = true;
			item.maxStack = 1;
			item.knockBack = 9f;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
		}

		private int useCool = 0;
		private int lungeChargeTimer = 0;

        public override void HoldItem(Player player)
        {
			MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
			VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
			if (player.whoAmI != item.owner || !vPlayer.zombie || (mPlayer.StandOut && !mPlayer.StandAutoMode))
				return;

			if (useCool > 0)
				useCool--;

            if (Main.mouseLeft && useCool <= 0)
            {
				lungeChargeTimer++;
				if (lungeChargeTimer > 180)
                {
					lungeChargeTimer = 180;
                }
			}
			if (!Main.mouseLeft && lungeChargeTimer > 0 && useCool <= 0)
            {
				useCool += item.useTime + (20 * (lungeChargeTimer / 30));
				int multiplier = lungeChargeTimer / 60;
				if (multiplier == 0)
                {
					multiplier = 1;
                }
				if (Main.MouseWorld.X - player.position.X >= 0)
				{
					player.direction = 1;
				}
				else
				{
					player.direction = -1;
				}
				player.immune = true;
				player.immuneTime = 20;
				Vector2 launchVector = Main.MouseWorld - player.position;
				launchVector.Normalize();
				launchVector *= multiplier * 6;
				player.velocity += launchVector;
				Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("VampiricSlash"), item.damage * multiplier, item.knockBack * multiplier, item.owner);
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 1, 1f, 0.6f);
				lungeChargeTimer = 0;
			}

			if (Main.mouseRight && useCool <= 0 && !player.HasBuff(mod.BuffType("KnifeAmalgamation")) && player.CountItem(mod.ItemType("Knife")) >= 16)
            {
				useCool += 30;
				player.AddBuff(mod.BuffType("KnifeAmalgamation"), 2 * 60 * 60);
				player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " has tried to stick too many knives into themselves!"), 24, -player.direction);
				for (int i = 0; i < 16; i++)
				{
					player.ConsumeItem(mod.ItemType("Knife"));
				}
			}

			bool specialJustPressed = false;
			if (!Main.dedServ)
				specialJustPressed = JoJoStands.SpecialHotKey.JustPressed;

			if (specialJustPressed && useCool <= 0 && player.HasBuff(mod.BuffType("KnifeAmalgamation")))
            {
				int knivesToThrow = 16;
				for (int i = 0; i < knivesToThrow; i++)
                {
					float angle = MathHelper.ToRadians((360f / knivesToThrow) * i);
					Vector2 knifePos = player.Center + (angle.ToRotationVector2() * player.height);
					Vector2 knifeVel = player.Center - knifePos;
					knifeVel.Normalize();
					knifeVel *= 14f;
					Projectile.NewProjectile(knifePos, knifeVel, mod.ProjectileType("Knife"), 47, 5f, player.whoAmI);
                }
				player.buffTime[player.FindBuffIndex(mod.BuffType("KnifeAmalgamation"))] -= 45 * 60;
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
