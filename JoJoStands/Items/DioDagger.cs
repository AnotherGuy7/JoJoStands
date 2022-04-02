using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class DioDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dio's Dagger");
			Tooltip.SetDefault("Right-click to stab yourself with this dagger and attract the attention of the zombies.");
		}

		public override void SetDefaults()
		{
			item.damage = 14;
			item.width = 28;
			item.height = 28;
			item.useTime = 10;
			item.useAnimation = 10;
			item.maxStack = 1;
			item.noUseGraphic = false;
			item.useStyle = ItemUseStyleID.Stabbing;
			item.UseSound = SoundID.Item1;
			item.rare = ItemRarityID.Green;
			item.value = Item.buyPrice(0, 0, 9, 50);
		}

        public override bool AltFunctionUse(Player player)
        {
			return true;
        }

        public override bool UseItem(Player player)
        {
			if (Main.mouseRight && !Main.dayTime && NPC.downedBoss1 && !JoJoStandsWorld.VampiricNight)
            {
				JoJoStandsWorld.VampiricNight = true;
				Main.NewText("Dio's Minions have arrived!", new Color(50, 255, 130));
				Main.PlaySound(15, (int)player.Center.X, (int)player.Center.Y, 0, 1f, -1.9f);
				player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " has succumbed to fate, just like a certain father once did."), 1, -player.direction);
			}
			return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 6);
			recipe.AddIngredient(ItemID.Ruby, 2);
			recipe.AddIngredient(ItemID.DemoniteBar, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 6);
			recipe.AddIngredient(ItemID.Ruby, 2);
			recipe.AddIngredient(ItemID.CrimtaneBar, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadBar, 6);
			recipe.AddIngredient(ItemID.Ruby, 2);
			recipe.AddIngredient(ItemID.DemoniteBar, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadBar, 6);
			recipe.AddIngredient(ItemID.Ruby, 2);
			recipe.AddIngredient(ItemID.CrimtaneBar, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}