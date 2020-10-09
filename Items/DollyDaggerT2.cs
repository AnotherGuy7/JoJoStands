using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class DollyDaggerT2 : ModItem
	{
		public override string Texture
		{
			get { return mod.Name + "/Items/DollyDaggerT1"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dolly Dagger (Tier 2)");
			Tooltip.SetDefault("As an item: Left-click to use this as a dagger to stab enemies and right-click to stab yourself and reflect damage to the nearest enemy!\nIn the Stand Slot: Equip it to nullify and reflect 70% of all damage!");
		}

		public override void SetDefaults()
		{
			item.damage = 63;
			item.width = 16;
			item.height = 16;
			item.useTime = 10;
			item.useAnimation = 10;
			item.maxStack = 1;
			item.noUseGraphic = false;
			item.useStyle = ItemUseStyleID.Stabbing;
			item.UseSound = SoundID.Item1;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.buyPrice(0, 2, 0, 0);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				int stabDamage = Main.rand.Next(50, 81);
				player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " couldn't reflect enough damage back."), stabDamage, player.direction);
				Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("DollyDaggerBeam"), stabDamage, item.knockBack, player.whoAmI);
			}
			else
			{
				item.damage = 63;
				item.useTime = 10;
				item.useAnimation = 10;
				item.useStyle = ItemUseStyleID.Stabbing;
				item.UseSound = SoundID.Item1;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 4);
			recipe.AddIngredient(mod.ItemType("WillToChange"), 2);
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}