using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class PressurizedBlood : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 8;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 360;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (Main.rand.Next(0, 1 + 1) == 0)
            {
                int newDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 28);
                Main.dust[newDust].noGravity = true;
            }
        }
    }
}