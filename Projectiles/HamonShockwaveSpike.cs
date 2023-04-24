using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HamonShockwaveSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 30;
            Projectile.penetrate = 3;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private int groundCheckTimer = 0;
        private float velocityMultiplier = 0.3f;
        private bool setPenetrate = false;

        public override void AI()
        {
            groundCheckTimer++;
            Projectile.timeLeft = 2;
            velocityMultiplier += 0.1f;
            if (!setPenetrate)
            {
                Projectile.penetrate = 3 * (int)Projectile.ai[1];
                setPenetrate = true;
            }

            Projectile.spriteDirection = Projectile.direction = (int)Projectile.ai[0];
            Projectile.velocity = new Vector2(1f * velocityMultiplier * Projectile.direction, 3f);
            if (groundCheckTimer >= 30)
            {
                if (!WorldGen.SolidTile((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f) + 1))
                    Projectile.Kill();

                groundCheckTimer = 0;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 169, 0f, 0f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (WorldGen.SolidTile((int)(Projectile.Center.X / 16f) + (1 * Projectile.direction), (int)(Projectile.Center.Y / 16f)))
            {
                Projectile.Kill();
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Sunburn>(), 4 * 60);
        }
    }
}