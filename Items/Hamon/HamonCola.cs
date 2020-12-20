using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
	public class HamonCola : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hamon-Infused Cola");
			Tooltip.SetDefault("Drink this hamon-infused cola to feel stronger or shoot it all at your enemies!");
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 10;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 3;
            item.value = Item.buyPrice(0, 0, 8, 50);
			item.rare = 3;
            item.damage = 34;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 3;
            item.knockBack = 3f;
            item.UseSound = null;
            item.shoot = mod.ProjectileType("HamonSodaBottleCap");
            item.shootSpeed = 24f;
            item.potion = false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
			if (player.altFunctionUse == 2)
            {
                item.damage = 34;
                item.useTime = 24;
                item.useAnimation = 24;
                item.useStyle = 3;
                item.knockBack = 3f;
                item.UseSound = null;
                item.shoot = mod.ProjectileType("HamonSodaBottleCap");
                item.shootSpeed = 24f;
                item.potion = false;
            }
            if (player.altFunctionUse != 2 && !player.HasBuff(BuffID.PotionSickness))
            {
                player.statLife += 75;
                player.HealEffect(75);
                player.AddBuff(BuffID.Swiftness, 1500);
                player.AddBuff(BuffID.Regeneration, 1500);
                item.UseSound = SoundID.Item3;
                item.potion = true;
                player.GetModPlayer<HamonPlayer>().amountOfHamon += 10;
            }
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.BottledHoney, 2);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
