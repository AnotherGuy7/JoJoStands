using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public class WitheringAbilities : VampireDamageClass
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Withering Abilities");
            Tooltip.SetDefault("Hold left-click to charge up a punch! Right-click to extend your arm and grab enemies to crush them in place!\nSpecial: Throw up acidic bile! Enemies hit with the bile will be marked for 45s.");
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 42;
            Item.width = 26;
            Item.height = 26;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.consumable = false;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.knockBack = 12f;
            Item.value = 0;
            Item.rare = ItemRarityID.Orange;
        }

        private int useCool = 0;
        private int punchChargeTimer = 0;

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (player.whoAmI != Main.myPlayer || !vPlayer.zombie || (mPlayer.standOut && !mPlayer.standAutoMode))
                return;

            if (useCool > 0)
                useCool--;

            vPlayer.enemyIgnoreItemInUse = true;
            if (Main.mouseLeft && useCool <= 0)
            {
                punchChargeTimer++;
                if (punchChargeTimer > 5 * 60)
                    punchChargeTimer = 5 * 60;

                player.velocity.X *= 0.8f;
                player.velocity.Y *= 0.9f;
            }
            if (!Main.mouseLeft && punchChargeTimer > 0 && useCool <= 0)
            {
                int multiplier = punchChargeTimer / 60;
                if (multiplier == 0)
                    multiplier = 1;

                player.direction = 1;
                if (Main.MouseWorld.X - player.position.X < 0)
                    player.direction = -1;

                Vector2 launchVector = Main.MouseWorld - player.position;
                launchVector.Normalize();
                launchVector *= multiplier * 4f;
                player.velocity += launchVector;
                useCool += Item.useTime + (6 * (punchChargeTimer / 30));
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<VampiricPunch>(), Item.damage * multiplier, Item.knockBack * multiplier, player.whoAmI);
                SoundEngine.PlaySound(2, (int)player.position.X, (int)player.position.Y, 1, 1f, 0.2f);
                punchChargeTimer = 0;
            }

            if (Main.mouseRight && player.ownedProjectileCounts[ModContent.ProjectileType<ExtendedZombieGrab>()] <= 0 && useCool <= 0)
            {
                useCool += 2 * 60;
                Vector2 shootVel = Main.MouseWorld - player.Center;
                shootVel.Normalize();
                shootVel *= 10f;
                int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, shootVel, ModContent.ProjectileType<ExtendedZombieGrab>(), Item.damage, 0f, player.whoAmI);
                Main.projectile[proj].netUpdate = true;
            }

            bool specialPressed = false;
            if (player.whoAmI == Main.myPlayer)
                specialPressed = JoJoStands.SpecialHotKey.JustPressed;

            if (specialPressed && useCool <= 0)
            {
                Vector2 shootVel = Main.MouseWorld - player.Center;
                if (shootVel == Vector2.Zero)
                    shootVel = new Vector2(0f, 1f);

                shootVel.Normalize();
                shootVel *= 8f;
                useCool += (2 * 60) + 30;

                float numberProjectiles = Main.rand.Next(4, 6 + 1);
                float rotation = MathHelper.ToRadians(30);
                for (int i = 0; i < numberProjectiles; i++)
                {
                    float randomSpeedOffset = (100f + Main.rand.NextFloat(-15f, 15f + 1f)) / 100f;
                    Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
                    perturbedSpeed *= randomSpeedOffset;
                    int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, perturbedSpeed, ModContent.ProjectileType<AcidicBile>(), (int)(Item.damage * 1.5f), 2f, player.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                }
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " overstepped their boundaries as a Zombie."), 14, player.direction);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddCondition(NetworkText.FromLiteral("ZombieRequirement"), r => !Main.gameMenu && Main.LocalPlayer.GetModPlayer<VampirePlayer>().zombie && Main.LocalPlayer.GetModPlayer<VampirePlayer>().HasSkill(Main.LocalPlayer, VampirePlayer.WitheringAbilities))
                .Register();
        }
    }
}
