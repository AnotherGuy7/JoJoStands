using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class RoadRoller : ModProjectile
    {
        private Vector2 velocityAdd = Vector2.Zero;
        private float damageMult = 1f;
        private bool landed = false;

        public override void SetDefaults()
        {
            projectile.width = 144;
            projectile.height = 74;
            projectile.aiStyle = 0;
            projectile.timeLeft = 1800;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (projectile.timeLeft < 256)
            {
                projectile.alpha = -projectile.timeLeft + 255;
            }
            projectile.spriteDirection = projectile.direction;
            projectile.rotation = (projectile.velocity * new Vector2(projectile.spriteDirection)).ToRotation();

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile otherProj = Main.projectile[p];
                if (p == projectile.whoAmI)
                    continue;

                if (otherProj.active && otherProj.type == mod.ProjectileType("Fists"))
                {
                    if (projectile.Hitbox.Intersects(otherProj.Hitbox) && mPlayer.timestopActive)
                    {
                        velocityAdd += otherProj.velocity / 75f;
                        if (damageMult <= 4f)
                        {
                            damageMult += otherProj.damage / 50;
                        }
                        Dust.NewDust(otherProj.position, projectile.width, projectile.height, DustID.FlameBurst, otherProj.velocity.X * -0.5f, otherProj.velocity.Y * -0.5f);
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
                        otherProj.Kill();
                    }
                }
            }

            if (landed && projectile.timeLeft < 180)
            {
                projectile.damage = 0;
            }
            if (!mPlayer.timestopActive)
            {
                projectile.velocity.Y += 0.1f;
                if (velocityAdd != Vector2.Zero)
                {
                    projectile.velocity += velocityAdd;
                    projectile.damage *= (int)damageMult;
                    damageMult = 1f;
                    velocityAdd = Vector2.Zero;
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.boss)
                damage *= 3;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            landed = true;
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            MyPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<MyPlayer>();
            return !mPlayer.timestopActive;
        }
    }
}