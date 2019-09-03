using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KingCrimsonT3 : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/KingCrimsonT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("King Crimson (Tier 3)");
			Tooltip.SetDefault("Donut enemies with a light-dash donut and right-click to do a more powerful donut! \nSpecial: Skip 5 seconds of time!");
		}

		public override void SetDefaults()
		{
			item.damage = 124;	//Mechs
			item.width = 32;
			item.height = 32;
			item.useTime = 30;
			item.useAnimation = 30;
			item.maxStack = 1;
            item.useStyle = 5;
            item.useTurn = true;
            item.noUseGraphic = true;
			item.knockBack = 4f;
			item.rare = 6;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
		}

        public override void HoldItem(Player player)
        {
            if (JoJoStands.ItemHotKey.JustPressed && !player.HasBuff(mod.BuffType("TimeCooldown")) && !player.HasBuff(mod.BuffType("SkippingTime")) && player.whoAmI == Main.myPlayer)
            {
                player.AddBuff(mod.BuffType("PreTimeSkip"), 10);             //make it last longer
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/TimeSkip"));
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.damage = 146;
                item.useTime = 60;
                item.useAnimation = 60;
                item.reuseDelay = 45;
                item.knockBack = 4f;
                item.noUseGraphic = true;
                item.UseSound = SoundID.Item1;
                item.shoot = mod.ProjectileType("KingCrimsonDonut");
                item.shootSpeed = 50f;
            }
            else
            {
                item.damage = 124;
                item.useTime = 30;
                item.useAnimation = 30;
                item.reuseDelay = 40;
                item.knockBack = 4f;
                item.UseSound = SoundID.Item1;
                item.noUseGraphic = true;
                item.shoot = mod.ProjectileType("KingCrimsonDonut");
                item.useTurn = true;
                item.shootSpeed = 50f;
                player.velocity.X = 5f * (float)player.direction;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("KingCrimsonT2"));
            recipe.AddIngredient(ItemID.SoulofFright, 4);
            recipe.AddIngredient(ItemID.SoulofSight, 6);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
