using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class AerosmithBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            Projectile.velocity.Y += 0.3f;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            var explosion = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ProjectileID.GrenadeIII, (int)(Projectile.ai[0] * Mplayer.standDamageBoosts), 8f, Projectile.owner);
            Main.projectile[explosion].timeLeft = 2;
            Main.projectile[explosion].netUpdate = true;
            SoundEngine.PlaySound(SoundID.Item62);
        }
    }
}