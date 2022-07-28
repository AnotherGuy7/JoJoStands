using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class ChainedClaw : HamonDamageClass
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Throw this chained claw to grab enemies and inject Hamon into them!\nRequires 5 or more Hamon to inject Hamon while an enemy is grabbed.\nSpecial: Hamon Breathing");
            SacrificeTotal = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 19;
            Item.width = 32;
            Item.height = 32;        //hitbox's width and height when the Item is in the world
            Item.useTime = 80;
            Item.useAnimation = 80;
            Item.maxStack = 1;
            Item.noUseGraphic = true;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.Pink;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<ChainedClawProjectile>();
            Item.shootSpeed = 9f;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<ChainedClawProjectile>()] == 0;
        }

        public override void HoldItem(Player player)
        {
            ChargeHamon();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Chain, 6)
                .AddRecipeGroup("JoJoStandsIron-TierBar", 3)
                .AddIngredient(ItemID.Sapphire)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
