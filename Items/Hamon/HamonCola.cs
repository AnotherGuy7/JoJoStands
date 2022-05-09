using JoJoStands.Projectiles;
using Terraria;
using Terraria.ID;
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
            Item.width = 10;
            Item.height = 10;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = 3;
            Item.value = Item.buyPrice(0, 0, 8, 50);
            Item.rare = 3;
            Item.damage = 34;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = 3;
            Item.knockBack = 3f;
            Item.UseSound = null;
            Item.shoot = ModContent.ProjectileType<HamonSodaBottleCap>();
            Item.shootSpeed = 24f;
            Item.potion = false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.damage = 34;
                Item.useTime = 24;
                Item.useAnimation = 24;
                Item.useStyle = 3;
                Item.knockBack = 3f;
                Item.UseSound = null;
                Item.shoot = ModContent.ProjectileType<HamonSodaBottleCap>();
                Item.shootSpeed = 24f;
                Item.potion = false;
            }
            if (player.altFunctionUse != 2 && !player.HasBuff(BuffID.PotionSickness))
            {
                player.statLife += 75;
                player.HealEffect(75);
                player.AddBuff(BuffID.Swiftness, 1500);
                player.AddBuff(BuffID.Regeneration, 1500);
                Item.UseSound = SoundID.Item3;
                Item.potion = true;
                player.GetModPlayer<HamonPlayer>().amountOfHamon += 10;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemID.BottledHoney, 2)
                .Register();
        }
    }
}
