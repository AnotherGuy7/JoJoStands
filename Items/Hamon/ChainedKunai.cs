using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class ChainedKunai : HamonDamageClass
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Swing around and throw this hamon-infused Kunai!\nSpecial: Hamon Breathing");
            SacrificeTotal = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 7;
            Item.width = 24;
            Item.height = 28;        //hitbox's width and height when the Item is in the world
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.maxStack = 1;
            Item.noUseGraphic = true;
            Item.knockBack = 3f;
            Item.rare = ItemRarityID.Pink;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<ChainedKunaiSwinging>();
            Item.shootSpeed = 2f;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<ChainedKunaiSwinging>()] == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<ChainedKunaiProjectile>()] == 0;
        }

        public override void HoldItem(Player player)
        {
            ChargeHamon();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WhiteString)
                .AddRecipeGroup("JoJoStandsIron-TierBar", 5)
                .AddIngredient(ItemID.Silk, 2)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 7)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
