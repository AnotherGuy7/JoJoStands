using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;

namespace JoJoStands.Projectiles
{
    public class StandBullet : ModProjectile 
    {
        public override string Texture { get { return "Terraria/Images/Projectile_" + ProjectileID.Bullet; } }

        public override void SetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Arrow;       //It's for bullets too
            Projectile.friendly = true; 
            Projectile.timeLeft = 600;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() * 0.4f);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                crit = true;
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.Next(1, 100 + 1) >= 60)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
            }
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}
