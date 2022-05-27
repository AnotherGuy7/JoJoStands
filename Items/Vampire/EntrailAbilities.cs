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
    public class EntrailAbilities : VampireDamageClass
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Entrail Abilities");
            Tooltip.SetDefault("Left-click to shoot out a sharp vein at your enemies! Right-click to send out a vein to entangle your enemies!\nSpecial: In exchange for some health, shoot an extremely pressurized blood stream in the direction of your mouse. Inflicts Lacerated! on enemies.");
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 58;
            Item.width = 26;
            Item.height = 26;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.consumable = false;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.knockBack = 12f;
            Item.value = 0;
            Item.rare = ItemRarityID.Orange;
        }

        private int useCool = 0;

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (player.whoAmI != Main.myPlayer || !vPlayer.zombie || (mPlayer.standOut && !mPlayer.standAutoMode))
                return;

            if (useCool > 0)
                useCool--;

            if (player.ownedProjectileCounts[ModContent.ProjectileType<VampiricVeinGrab>()] > 0)
                useCool = 2;

            vPlayer.enemyIgnoreItemInUse = true;
            if (Main.mouseLeft && useCool <= 0)
            {
                player.direction = 1;
                if (Main.MouseWorld.X - player.position.X < 0)
                    player.direction = -1;

                useCool += Item.useTime;
                Vector2 shootVel = Main.MouseWorld - player.position;
                shootVel.Normalize();
                shootVel *= 12f;
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, shootVel, ModContent.ProjectileType<VampiricVeinSpike>(), Item.damage, Item.knockBack, player.whoAmI);
                SoundStyle item17 = SoundID.Item17;
                item17.Pitch = -0.6f;
                SoundEngine.PlaySound(item17, player.Center);
            }

            if (Main.mouseRight && useCool <= 0)
            {
                useCool += 40;
                Vector2 shootVel = Main.MouseWorld - player.Center;
                shootVel.Normalize();
                shootVel *= 12f;
                int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, shootVel, ModContent.ProjectileType<VampiricVeinGrab>(), (int)(Item.damage * 1.2f), 0f, player.whoAmI);
                Main.projectile[proj].netUpdate = true;
                SoundStyle item17 = SoundID.Item17;
                item17.Pitch = -0.6f;
                SoundEngine.PlaySound(item17, player.Center);
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
                shootVel *= 16f;
                useCool += 3 * 60;

                float numberProjectiles = 4;
                float rotation = MathHelper.ToRadians(30);
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
                    int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, perturbedSpeed, ModContent.ProjectileType<PressurizedBlood>(), Item.damage * 5, 9f, player.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                }
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " overstepped their boundaries as a Zombie."), 32, player.direction);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddCondition(NetworkText.FromLiteral("ZombieRequirement"), r => !Main.gameMenu && Main.LocalPlayer.GetModPlayer<VampirePlayer>().zombie && Main.LocalPlayer.GetModPlayer<VampirePlayer>().HasSkill(Main.LocalPlayer, VampirePlayer.EntrailAbilities))
                .Register();
        }
    }
}
