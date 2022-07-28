using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class MetallicNunchucks : HamonDamageClass
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Swing around these metallic nunchucks and then deal heavy blows to enemies!\nSpecial: Hamon Breathing");
            SacrificeTotal = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 65;
            Item.width = 24;
            Item.height = 28;        //hitbox's width and height when the Item is in the world
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.maxStack = 1;
            Item.noUseGraphic = true;
            Item.knockBack = 12f;
            Item.rare = ItemRarityID.Orange;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<MetallicNunchucksSwinging>();
            Item.shootSpeed = 10f;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<MetallicNunchucksSwinging>()] == 0 && player.ownedProjectileCounts[ModContent.ProjectileType<MetallicNunchucksProjectile>()] == 0;
        }

        public override void HoldItem(Player player)
        {
            ChargeHamon();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Chain, 5)
                .AddRecipeGroup("JoJoStandsIron-TierBar", 8)
                .AddRecipeGroup("JoJoStandsEvilBar", 6)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
