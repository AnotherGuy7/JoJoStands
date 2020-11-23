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
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (projectile.timeLeft < 256)
            {
                projectile.alpha = -projectile.timeLeft + 255;
            }
            projectile.spriteDirection = projectile.direction;
            projectile.rotation = (projectile.velocity * new Vector2(projectile.spriteDirection, projectile.spriteDirection)).ToRotation();
            //Main.NewText(projectile.rotation + "; " + projectile.direction + "; " + projectile.spriteDirection);

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile otherProj = Main.projectile[p];
                if (otherProj.active && otherProj.type == mod.ProjectileType("Fists"))
                {
                    if (projectile.Hitbox.Intersects(otherProj.Hitbox) && modPlayer.TheWorldEffect)
                    {
                        velocityAdd.X += otherProj.velocity.X / 75f;
                        velocityAdd.Y += otherProj.velocity.Y / 75f;
                        if (damageMult <= 4f)
                        {
                            damageMult += otherProj.damage / 50;
                        }
                        //Main.NewText("Mult: " + damageMult + "; Current Vel: " + projectile.velocity + "; X: " + velocityAdd.X + "; Y: " + velocityAdd.Y);
                        Dust.NewDust(otherProj.position, projectile.width, projectile.height, DustID.FlameBurst, otherProj.velocity.X * -0.5f, otherProj.velocity.Y * -0.5f);
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
                        otherProj.Kill();
                        //Main.NewText(projectile.velocity + velocityAdd);
                    }
                }
            }

            if (landed && projectile.timeLeft < 180)
            {
                projectile.damage = 0;
            }
            if (!modPlayer.TheWorldEffect)
            {
                projectile.velocity.Y += 0.1f;
                if (velocityAdd != Vector2.Zero)
                {
                    //Main.NewText("Vel: " + projectile.velocity + "; D: " + projectile.damage);
                    projectile.velocity += velocityAdd;
                    projectile.damage *= (int)damageMult;
                    //Main.NewText("New Vel: " + projectile.velocity + "; New D: " + projectile.damage);
                    damageMult = 1f;
                    velocityAdd = Vector2.Zero;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.boss)
            {
                target.StrikeNPC(damage * 3, projectile.knockBack, projectile.direction);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            landed = true;
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            return !modPlayer.TheWorldEffect;
        }
    }
}