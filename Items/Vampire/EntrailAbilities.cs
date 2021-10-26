using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

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
            item.damage = 35;
            item.width = 26;
            item.height = 26;
            item.useTime = 30;
            item.useAnimation = 30;
            item.consumable = false;
            item.noUseGraphic = true;
            item.maxStack = 1;
            item.knockBack = 12f;
            item.value = 0;
            item.rare = ItemRarityID.Orange;
        }

        private int useCool = 0;

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (player.whoAmI != item.owner || !vPlayer.zombie || (mPlayer.standOut && !mPlayer.standAutoMode))
                return;

            if (useCool > 0)
                useCool--;

            if (player.ownedProjectileCounts[mod.ProjectileType("VampiricVeinGrab")] > 0)
                useCool = 2;

            vPlayer.enemyIgnoreItemInUse = true;
            if (Main.mouseLeft && useCool <= 0)
            {
                player.direction = 1;
                if (Main.MouseWorld.X - player.position.X < 0)
                {
                    player.direction = -1;
                }

                useCool += item.useTime;
                Vector2 shootVel = Main.MouseWorld - player.position;
                shootVel.Normalize();
                shootVel *= 12f;
                Projectile.NewProjectile(player.Center, shootVel, mod.ProjectileType("VampiricVeinSpike"), item.damage, item.knockBack, item.owner);
                Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 17, 1f, -0.6f);
            }

            if (Main.mouseRight && useCool <= 0)
            {
                useCool += 40;
                Vector2 shootVel = Main.MouseWorld - player.Center;
                shootVel.Normalize();
                shootVel *= 12f;
                int proj = Projectile.NewProjectile(player.Center, shootVel, mod.ProjectileType("VampiricVeinGrab"), (int)(item.damage * 1.2f), 0f, player.whoAmI);
                Main.projectile[proj].netUpdate = true;
                Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 17, 1f, -0.6f);
            }

            bool specialPressed = false;
            if (player.whoAmI == Main.myPlayer)
                specialPressed = JoJoStands.SpecialHotKey.JustPressed;

            if (specialPressed && useCool <= 0)
            {
                Vector2 shootVel = Main.MouseWorld - player.Center;
                if (shootVel == Vector2.Zero)
                {
                    shootVel = new Vector2(0f, 1f);
                }
                shootVel.Normalize();
                shootVel *= 16f;
                useCool += 3 * 60;

                float numberProjectiles = 4;
                float rotation = MathHelper.ToRadians(30);
                for (int i = 0; i < numberProjectiles; i++)
                {
                    Vector2 perturbedSpeed = shootVel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
                    int proj = Projectile.NewProjectile(player.Center, perturbedSpeed, mod.ProjectileType("PressurizedBlood"), item.damage * 5, 9f, player.whoAmI);
                    Main.projectile[proj].netUpdate = true;
                }
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " overstepped their boundaries as a Zombie."), 32, player.direction);
            }
        }

        public override void AddRecipes()
        {
            VampiricItemRecipe recipe = new VampiricItemRecipe(mod, item.type);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
