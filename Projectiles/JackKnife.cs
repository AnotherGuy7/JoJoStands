using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class JackKnife : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.timeLeft = 1800;
            projectile.friendly = false;
            projectile.tileCollide = true;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 2;
        }

        private int expertboost = 1;

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.velocity.Y += 0.05f;

            if (Main.expertMode)
            {
                expertboost = 2;
            }

            projectile.damage = 6 * expertboost;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(SoundID.Item10, projectile.position);
        }
    }
}