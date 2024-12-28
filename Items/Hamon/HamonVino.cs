using JoJoStands.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class HamonVino : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hamon-Infused Vino");
            // Tooltip.SetDefault("Drink this hamon-infused vino to feel stronger and healthier or shoot it all at your enemies!");
            Item.ResearchUnlockCount = 10;
        }

        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.value = Item.buyPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.Orange;
            Item.knockBack = 3f;
            Item.UseSound = null;
            Item.shoot = ModContent.ProjectileType<HamonVinoBottleCap>();
            Item.potion = false;
            Item.shootSpeed = 24f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.damage = 42;
                Item.useTime = 24;
                Item.useAnimation = 24;
                Item.useStyle = ItemUseStyleID.Thrust;
                Item.knockBack = 3f;
                Item.UseSound = null;
                Item.shoot = ModContent.ProjectileType<HamonVinoBottleCap>();
                Item.potion = false;
                Item.shootSpeed = 24f;
            }
            if (player.altFunctionUse != 2 && !player.HasBuff(BuffID.PotionSickness))
            {
                Item.potion = true;
                Item.UseSound = SoundID.Item3;
                player.statLife += 125;
                player.HealEffect(75);
                player.AddBuff(BuffID.Swiftness, 1650);
                player.AddBuff(BuffID.Regeneration, 1650);
                player.AddBuff(BuffID.ManaRegeneration, 1650);
                player.GetModPlayer<HamonPlayer>().amountOfHamon += 25;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemID.BottledHoney, 3)
                .AddIngredient(ItemID.Ale)
                .AddIngredient(ItemID.JungleSpores, 2)
                .AddIngredient(ItemID.Glass)
                .AddTile(TileID.Kegs)
                .Register();
        }
    }
}
