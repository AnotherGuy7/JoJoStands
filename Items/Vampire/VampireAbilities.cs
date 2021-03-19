using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
	public class VampireAbilities : VampireDamageClass
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie Abilities");
			Tooltip.SetDefault("Left-click to lunge and hold right-click to grab an enemy and suck their blood!");
		}

		public override void SafeSetDefaults()
		{
			item.damage = 51;
			item.width = 28;
			item.height = 30;
			item.useTime = 40;
			item.useAnimation = 40;
            item.consumable = false;
            item.noUseGraphic = true;
			item.maxStack = 1;
			item.knockBack = 9f;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
		}

		private int useCool = 0;
		private int lungeChargeTimer = 0;
		private bool enemyBeingGrabbed = false;
		private int heldEnemyIndex = -1;
		private int heldEnemyTimer = 0;

        public override void HoldItem(Player player)
        {
			VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
			if (player.whoAmI != item.owner || !vPlayer.zombie)
				return;

			if (useCool > 0)
				useCool--;

			vPlayer.enemyIgnoreItemInUse = true;
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
				useCool += 40 + (20 * (lungeChargeTimer / 30));
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
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 1, 1f, 0.2f);
				lungeChargeTimer = 0;
			}
			if (Main.mouseRight && useCool <= 0)
            {
				if (!enemyBeingGrabbed)
				{
					for (int n = 0; n < Main.maxNPCs; n++)
					{
						NPC npc = Main.npc[n];
						if (npc.active)
						{
							if (player.Distance(npc.Center) <= 30f && !npc.boss && !npc.immortal && !npc.hide)
							{
								enemyBeingGrabbed = true;
								heldEnemyIndex = npc.whoAmI;
								break;
							}
						}
					}
				}
				else
                {
					NPC heldNPC = Main.npc[heldEnemyIndex];
					if (!heldNPC.active)
					{
						vPlayer.enemyToIgnoreDamageFromIndex = -1;
						enemyBeingGrabbed = false;
						heldEnemyIndex = -1;
						heldEnemyTimer = 0;
						useCool += 120;
						return;
					}

					player.controlUp = false;
					player.controlDown = false;
					player.controlLeft = false;
					player.controlRight = false;
					player.controlJump = false;
					player.velocity = Vector2.Zero;
					player.itemRotation = MathHelper.ToRadians(30f);

					heldNPC.direction = -player.direction;
					heldNPC.position = player.position + new Vector2(5f * player.direction, -2f - heldNPC.height / 3f);
					heldNPC.velocity = Vector2.Zero;
					vPlayer.enemyToIgnoreDamageFromIndex = heldNPC.whoAmI;

					heldEnemyTimer++;
					if (heldEnemyTimer >= 60)
                    {
						int suckAmount = (int)(heldNPC.lifeMax * 0.05f);
						player.HealEffect(suckAmount);
						player.statLife += suckAmount;
						heldNPC.StrikeNPC(suckAmount, 0f, player.direction);
						Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 3, 1f, -0.8f);
						heldEnemyTimer = 0;
                    }
				}
			}
			else
            {
				enemyBeingGrabbed = false;
				heldEnemyIndex = -1;
				heldEnemyTimer = 0;
				vPlayer.enemyToIgnoreDamageFromIndex = -1;
			}
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
