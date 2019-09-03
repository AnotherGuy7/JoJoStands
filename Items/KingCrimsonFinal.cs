using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KingCrimsonFinal : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/KingCrimsonT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("King Crimson (Final Tier)");
			Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to do a more powerful donut! \nSpecial: Skip 10 seconds of time!");
		}

		public override void SetDefaults()      //maybe make an Epitaph special? (What'll it do is the question I'm thinking right now)
		{
			item.damage = 151;	//Plantera
			item.width = 32;
			item.height = 32;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 5;
			item.maxStack = 1;
            item.knockBack = 5f;
            item.rare = 6;
            item.noUseGraphic = true;
            item.useTurn = true;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
			item.shoot = mod.ProjectileType("KingCrimsonFist");
			item.shootSpeed = 50f;
		}

        public override void HoldItem(Player player)
        {
            if (JoJoStands.ItemHotKey.JustPressed && !player.HasBuff(mod.BuffType("TimeCooldown")) && !player.HasBuff(mod.BuffType("SkippingTime")) && player.whoAmI == Main.myPlayer)
            {
                player.AddBuff(mod.BuffType("PreTimeSkip"), 10);
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
                item.damage = 172;
                item.useTime = 60;
                item.useAnimation = 60;
                item.reuseDelay = 40;
                item.knockBack = 4f;
                item.noUseGraphic = true;
                item.UseSound = SoundID.Item1;
                item.shoot = mod.ProjectileType("KingCrimsonDonut");
                item.shootSpeed = 50f;
            }
            else
            {
                item.damage = 151;
                item.useTime = 12;
                item.useAnimation = 12;
                item.reuseDelay = 35;
                item.knockBack = 5f;
                item.UseSound = SoundID.Item1;
                item.shoot = mod.ProjectileType("KingCrimsonDonut");
                item.useTurn = true;
                item.noUseGraphic = true;
                item.shootSpeed = 50f;
                player.velocity.X = 5f * (float)player.direction;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("KingCrimsonT3"));
            recipe.AddIngredient(ItemID.ChlorophyteBar, 13);
            recipe.AddIngredient(ItemID.GoldCrown);
            recipe.AddIngredient(mod.ItemType("SoulofTime"), 2);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
