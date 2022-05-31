using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class SoapyGlove : HamonDamageClass
    {
        public override bool affectedByHamonScaling => true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soapy Glove");
            Tooltip.SetDefault("Shoot controllable bubbles! \nExperience goes up after each conquer... \nRight-click requires more than 3 hamon\nSpecial: Hamon Breathing");
            SacrificeTotal = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 9;
            Item.width = 30;
            Item.height = 30;        //hitbox's width and height when the Item is in the world
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.maxStack = 1;
            Item.knockBack = 1f;
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item85;
            Item.shoot = ModContent.ProjectileType<HamonBubble>();
            Item.shootSpeed = 4f;
            Item.useTurn = true;
            Item.noWet = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            if (player.altFunctionUse == 2 && player.ownedProjectileCounts[ModContent.ProjectileType<CutterHamonBubble>()] <= 0 && hamonPlayer.amountOfHamon < 3)
                return false;

            if (player.altFunctionUse != 2 && hamonPlayer.amountOfHamon < 1)
                return false;

            if (player.altFunctionUse == 2)
            {
                Item.damage = 15;
                Item.knockBack = 8f;
                Item.shoot = ModContent.ProjectileType<CutterHamonBubble>();
            }
            if (player.altFunctionUse != 2)
            {
                Item.damage = 9;
                Item.knockBack = 1f;
                Item.shoot = ModContent.ProjectileType<HamonBubble>();
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (player.altFunctionUse == 2 && hamonPlayer.amountOfHamon >= 3)
            {
                damage += 6;
                type = ModContent.ProjectileType<CutterHamonBubble>();
                knockback = 8f;
                Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, damage, knockback, player.whoAmI);
                return false;
            }
            if (player.altFunctionUse != 2 && hamonPlayer.amountOfHamon > 1)
            {
                if (player.statLife <= (player.statLifeMax * 0.05f))
                {
                    damage += 24;
                    type = ModContent.ProjectileType<HamonBloodBubble>();
                    hamonPlayer.amountOfHamon -= 1;
                    Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, damage, knockback, player.whoAmI);
                    return false;
                }
                else
                {
                    type = ModContent.ProjectileType<HamonBubble>();
                    hamonPlayer.amountOfHamon -= 1;
                    Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, damage, knockback, player.whoAmI);
                    return false;
                }
            }
            return true;
        }

        public override void HoldItem(Player player)
        {
            ChargeHamon();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 14)
                .Register();
        }
    }
}
