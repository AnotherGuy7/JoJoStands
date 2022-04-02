using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HamonShockwaveSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 16;
            projectile.aiStyle = 0;
            projectile.timeLeft = 30;
            projectile.penetrate = 3;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        private int groundCheckTimer = 0;
        private float velocityMultiplier = 0.3f;
        private bool setPenetrate = false;

        public override void AI()
        {
            groundCheckTimer++;
            projectile.timeLeft = 2;
            velocityMultiplier += 0.1f;
            if (!setPenetrate)
            {
                projectile.penetrate = 3 * (int)projectile.ai[1];
                setPenetrate = true;
            }

            projectile.spriteDirection = projectile.direction = (int)projectile.ai[0];
            projectile.velocity = new Vector2(1f * velocityMultiplier * projectile.direction, 3f);
            if (groundCheckTimer >= 30)
            {
                if (!WorldGen.SolidTile((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f) + 1))
                {
                    projectile.Kill();
                }
                groundCheckTimer = 0;
            }

            projectile.frameCounter++;
            if (projectile.frameCounter >= 10)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }

            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 169, 0f, 0f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (WorldGen.SolidTile((int)(projectile.Center.X / 16f) + (1 * projectile.direction), (int)(projectile.Center.Y / 16f)))
            {
                projectile.Kill();
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("Sunburn"), 4 * 60);
        }
    }
}