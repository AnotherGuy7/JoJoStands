using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace JoJoStands.Projectiles
{
    public class StandBullet : ModProjectile 
    {
        public override string Texture { get { return "Terraria/Images/Projectile_" + ProjectileID.Bullet; } }
        public override void SetDefaults()
        {
            Projectile.aiStyle = 0; 
            Projectile.friendly = true; 
            Projectile.timeLeft = 600;
            AIType = ProjectileID.Bullet;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
                crit = true;
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}
