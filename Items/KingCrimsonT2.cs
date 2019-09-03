using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KingCrimsonT2 : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/KingCrimsonT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("King Crimson (Tier 2)");
			Tooltip.SetDefault("Donut enemies with a light-dash donut!! \nSpecial: Skip 2 seconds of time!");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.useStyle = 5;
			item.maxStack = 1;
			item.rare = 6;
            item.damage = 89;
            item.useTime = 40;
            item.useAnimation = 40;
            item.reuseDelay = 45;
            item.knockBack = 4f;
            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("KingCrimsonDonut");
            item.useTurn = true;
            item.autoReuse = true;
            item.noUseGraphic = true;
            item.shootSpeed = 50f;
        }

        public override void HoldItem(Player player)
        {
            if (JoJoStands.ItemHotKey.JustPressed && !player.HasBuff(mod.BuffType("TimeCooldown")) && !player.HasBuff(mod.BuffType("SkippingTime")) && player.whoAmI == Main.myPlayer)
            {
                player.AddBuff(mod.BuffType("PreTimeSkip"), 10);             //make it last longer
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/TimeSkip"));
            }
        }

        public override bool CanUseItem(Player player)      //UseItem wasn't doing the dashes?
        {
            player.velocity.X = 4f * player.direction;
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("KingCrimsonT1"));
            recipe.AddIngredient(ItemID.Hellstone, 15);
            recipe.AddIngredient(ItemID.CrimtaneBar, 3);
			recipe.SetResult(this);
			recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("KingCrimsonT1"));
            recipe.AddIngredient(ItemID.Hellstone, 15);
            recipe.AddIngredient(ItemID.DemoniteBar, 3);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
