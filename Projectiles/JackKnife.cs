using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class JackKnife : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1800;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 2;
        }

        private int expertboost = 1;

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity.Y += 0.05f;

            if (Main.expertMode)
            {
                expertboost = 2;
            }

            Projectile.damage = 6 * expertboost;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}