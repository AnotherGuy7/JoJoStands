using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
	public class HamonTequila : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hamon-Infused Tequila");
			Tooltip.SetDefault("Drink this hamon-infused tequila to feel stronger, healthier, and tougher... or shoot it all at your enemies!");
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 10;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 3;
            item.value = Item.buyPrice(0, 6, 48, 62);
			item.rare = 3;
            item.damage = 57;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 3;
            item.knockBack = 4f;
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
                item.damage = 57;
                item.useTime = 24;
                item.useAnimation = 24;
                item.useStyle = 3;
                item.knockBack = 4f;
                item.UseSound = null;
                item.shoot = mod.ProjectileType("HamonSodaBottleCap");
                item.shootSpeed = 24f;
                item.potion = false;
            }
            if (player.altFunctionUse != 2 && !player.HasBuff(BuffID.PotionSickness))
            {
                player.statLife += 175;
                player.HealEffect(75);
                player.AddBuff(BuffID.Swiftness, 1800);
                player.AddBuff(BuffID.Regeneration, 1800);
                player.AddBuff(BuffID.ManaRegeneration, 1800);
                player.AddBuff(BuffID.Ironskin, 1800);
                item.UseSound = SoundID.Item3;
                item.potion = true;
                player.GetModPlayer<MyPlayer>().HamonCounter += 40;
            }
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.BottledHoney, 3);
            recipe.AddIngredient(ItemID.Ale);
            recipe.AddIngredient(ItemID.Ichor, 2);
            recipe.AddIngredient(ItemID.Glass, 2);
            recipe.SetResult(this);
            recipe.AddTile(TileID.Kegs);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.BottledHoney, 3);
            recipe.AddIngredient(ItemID.Ale);
            recipe.AddIngredient(ItemID.CursedFlame, 2);
            recipe.AddIngredient(ItemID.Glass, 2);
            recipe.AddTile(TileID.Kegs);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
