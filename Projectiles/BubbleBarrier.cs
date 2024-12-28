using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class BubbleBarrier : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            Projectile.Center = player.Center;

            mPlayer.softAndWetBubbleRotation += 1;
            if (player.velocity.X != 0f)
                mPlayer.softAndWetBubbleRotation += (int)player.velocity.X * 2;

            for (int p = 0; p < Main.maxProjectiles; p++)
            {
                Projectile otherProj = Main.projectile[p];
                if (otherProj.active && otherProj.hostile && otherProj.Hitbox.Intersects(Projectile.Hitbox))
                {
                    otherProj.owner = Projectile.owner;
                    otherProj.hostile = false;
                    otherProj.friendly = true;
                    otherProj.velocity *= -1f;
                    SoundEngine.PlaySound(SoundID.SplashWeak, Projectile.Center);
                }
            }
        }
    }
}
   