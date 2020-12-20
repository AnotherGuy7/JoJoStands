using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
	public class HamonVino : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hamon-Infused Vino");
			Tooltip.SetDefault("Drink this hamon-infused vino to feel stronger and healthier or shoot it all at your enemies!");
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 10;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 3;
            item.value = Item.buyPrice(0, 2, 18, 68);
			item.rare = 3;
            item.damage = 42;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 3;
            item.knockBack = 3f;
            item.UseSound = null;
            item.shoot = mod.ProjectileType("HamonVinoBottleCap");
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
                item.damage = 42;
                item.useTime = 24;
                item.useAnimation = 24;
                item.useStyle = 3;
                item.knockBack = 3f;
                item.UseSound = null;
                item.shoot = mod.ProjectileType("HamonVinoBottleCap");
                item.shootSpeed = 24f;
                item.potion = false;
            }
            if (player.altFunctionUse != 2 && !player.HasBuff(BuffID.PotionSickness))
            {
                player.statLife += 125;
                player.HealEffect(75);
                player.AddBuff(BuffID.Swiftness, 1650);
                player.AddBuff(BuffID.Regeneration, 1650);
                player.AddBuff(BuffID.ManaRegeneration, 1650);
                item.UseSound = SoundID.Item3;
                item.potion = true;
                player.GetModPlayer<HamonPlayer>().amountOfHamon += 25;
            }
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.BottledHoney, 3);
            recipe.AddIngredient(ItemID.Ale);
            recipe.AddIngredient(ItemID.JungleSpores, 2);
            recipe.AddIngredient(ItemID.Glass);
            recipe.AddTile(TileID.Kegs);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
