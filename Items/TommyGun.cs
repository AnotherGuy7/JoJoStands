using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TommyGun : ModItem
    {
        private int shotsFired = 0;
        private int decayTimer = 0;
        private bool inForcedReload = false;
        private int reloadTimer = 0;

        private const int ReloadDuration = 60;
        private const int DrumCapacity = 150;

        private const float SpreadMin = 10f;
        private const float SpreadMax = 30f;

        private const float SpinRampShots = 42f;

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 22;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 28f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override void HoldItem(Player player)
        {
            if (inForcedReload)
            {
                reloadTimer--;

                if (reloadTimer <= 0)
                {
                    inForcedReload = false;
                    shotsFired = 0;
                }
                return;
            }

            bool isFiring = player.channel && player.itemAnimation > 0;

            float spinProgress = System.Math.Min(shotsFired / SpinRampShots, 1f);
            int maxAllowedTime = (int)MathHelper.Lerp(12f, 3f, spinProgress);

            if (player.itemTime > maxAllowedTime)
                player.itemTime = maxAllowedTime;

            if (player.itemAnimation > maxAllowedTime)
                player.itemAnimation = maxAllowedTime;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (inForcedReload)
                return false;

            shotsFired++;

            if (shotsFired >= DrumCapacity)
            {
                inForcedReload = true;
                reloadTimer = ReloadDuration;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, player.position);
            }

            float spinProgress = System.Math.Min(shotsFired / SpinRampShots, 1f);
            float spread = MathHelper.ToRadians(MathHelper.Lerp(SpreadMin, SpreadMax, spinProgress));
            Vector2 newVelocity = velocity.RotatedByRandom(spread);

            int projIndex = Projectile.NewProjectile(source, position, newVelocity,
                type, damage, knockback, player.whoAmI);

            if (projIndex >= 0 && projIndex < Main.maxProjectiles)
            {
                Main.projectile[projIndex]
                    .GetGlobalProjectile<JoJoGlobalProjectile>()
                    .firedByTommyGun = true;
            }

            return false;
        }

        public override bool? UseItem(Player player)
        {
            if (inForcedReload) return false;
            return null;
        }

        public override bool CanUseItem(Player player)
        {
            if (inForcedReload)
                return false;
            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 18)
                .AddIngredient(ItemID.IllegalGunParts)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}