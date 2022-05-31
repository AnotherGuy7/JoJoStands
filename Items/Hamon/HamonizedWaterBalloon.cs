using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class HamonizedWaterBalloon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Left-click to throw this Hamon-Infused Balloon and right-click to drop it as a trap!\nRequires 4 or more Hamon to be used.\nSpecial: Hamon Breathing");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.width = 30;
            Item.height = 28;
            Item.useTime = 75;
            Item.useAnimation = 75;
            Item.maxStack = 999;
            Item.knockBack = 5f;
            Item.rare = ItemRarityID.Pink;
            Item.noWet = true;
            Item.useTurn = true;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<HamonizedWaterBalloonProjectile>();
            Item.shootSpeed = 9f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<HamonPlayer>().amountOfHamon >= 4;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
                player.ConsumeItem(Item.type);

            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                velocity *= 0.5f;
                Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, damage, knockback, player.whoAmI, 1f);
            }
            else
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), position, velocity, type, damage, knockback, player.whoAmI, 0f);
            }
            player.GetModPlayer<HamonPlayer>().amountOfHamon -= 4;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddIngredient(ModContent.ItemType<SunDroplet>())
                .AddIngredient(ItemID.BottledWater)
                .Register();
        }
    }
}
