using JoJoStands.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class HamonTequila : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hamon-Infused Tequila");
            Tooltip.SetDefault("Drink this hamon-infused tequila to feel stronger, healthier, and tougher... or shoot it all at your enemies!");
            SacrificeTotal = 5;
        }

        public override void SetDefaults()
        {
            Item.damage = 57;
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.value = Item.buyPrice(gold: 7, silver: 50);
            Item.rare = ItemRarityID.Orange;
            Item.knockBack = 4f;
            Item.UseSound = null;
            Item.shoot = ModContent.ProjectileType<HamonTequilaBottleCap>();
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
                Item.damage = 57;
                Item.useTime = 24;
                Item.useAnimation = 24;
                Item.useStyle = ItemUseStyleID.Thrust;
                Item.knockBack = 4f;
                Item.UseSound = null;
                Item.shoot = ModContent.ProjectileType<HamonTequilaBottleCap>();
                Item.shootSpeed = 24f;
                Item.potion = false;
            }
            if (player.altFunctionUse != 2 && !player.HasBuff(BuffID.PotionSickness))
            {
                Item.potion = true;
                Item.UseSound = SoundID.Item3;
                player.statLife += 175;
                player.HealEffect(75);
                player.AddBuff(BuffID.Swiftness, 1800);
                player.AddBuff(BuffID.Regeneration, 1800);
                player.AddBuff(BuffID.ManaRegeneration, 1800);
                player.AddBuff(BuffID.Ironskin, 1800);
                player.GetModPlayer<HamonPlayer>().amountOfHamon += 40;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemID.BottledHoney, 3)
                .AddIngredient(ItemID.Ale)
                .AddIngredient(ItemID.Ichor, 2)
                .AddIngredient(ItemID.Glass, 2)
                .AddTile(TileID.Kegs)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.BottledWater)
                .AddIngredient(ItemID.BottledHoney, 3)
                .AddIngredient(ItemID.Ale)
                .AddIngredient(ItemID.CursedFlame, 2)
                .AddIngredient(ItemID.Glass, 2)
                .AddTile(TileID.Kegs)
                .Register();
        }
    }
}
