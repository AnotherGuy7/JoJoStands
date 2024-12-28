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

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
        }

        public override void OnKill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }
    }
}
