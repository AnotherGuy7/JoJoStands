using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class Clackers : HamonDamageClass
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clacker Balls");
            Tooltip.SetDefault("These clackers are now deadly weapons while infused with Hamon.\nRight-click requires more than 5 hamon\nSpecial: Hamon Breathing");
            SacrificeTotal = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 16;
            Item.width = 28;
            Item.height = 32;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.knockBack = 2f;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 8f;
            Item.useTurn = true;
            Item.noWet = true;
        }

        public override void HoldItem(Player player)
        {
            ChargeHamon();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (player.altFunctionUse == 2 && hamonPlayer.amountOfHamon >= 5)
            {
                damage = (int)(damage * 1.5f);
                velocity *= 1.25f;
                type = ModContent.ProjectileType<ChargedClackerProjectile>();
                hamonPlayer.amountOfHamon -= 5;
                Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, damage, knockback, player.whoAmI);
                return false;
            }

            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            if ((player.ownedProjectileCounts[ModContent.ProjectileType<ClackerProjectile>()] >= 1) || (player.ownedProjectileCounts[ModContent.ProjectileType<ChargedClackerProjectile>()] >= 1))
                return false;

            if (player.altFunctionUse != 2 || (player.altFunctionUse == 2 && hamonPlayer.amountOfHamon <= 4))
                Item.shoot = ModContent.ProjectileType<ClackerProjectile>();

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("JoJoStandsIron-TierBar", 30)
                .AddIngredient(ItemID.Cobweb, 25)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 45)
                .Register();
        }
    }
}
