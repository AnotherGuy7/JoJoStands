
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class TuskAct1 : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tusk (ACT 1)");
			Tooltip.SetDefault("Shoot nails at enemies!");
		}

		public override void SetDefaults()
		{
			item.damage = 21;
			item.width = 32;
			item.height = 32;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = 5;
			item.knockBack = 4;
			item.rare = 6;
            item.UseSound = SoundID.Item67;
			item.autoReuse = true;
            item.shoot = mod.ProjectileType("Nail");
			item.maxStack = 1;
            item.shootSpeed = 30f;
			item.channel = true;
			item.noUseGraphic = true;
            MyPlayer.standTier1List.Add(mod.ItemType(Name));
        }

		public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.TuskActNumber == 1 && player.whoAmI == Main.myPlayer)
            {
                mPlayer.TuskAct1Pet = true;
                if (player.ownedProjectileCounts[mod.ProjectileType("TuskAct1Pet")] <= 0 && mPlayer.TuskAct1Pet)
                {
                    Projectile.NewProjectile(player.position, player.velocity, mod.ProjectileType("TuskAct1Pet"), 0, 0f, Main.myPlayer);
                }
            }
        }

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.AddTile(mod.TileType("RemixTableTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}