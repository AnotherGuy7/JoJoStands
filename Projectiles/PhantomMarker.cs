using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class PhantomMarker : ModProjectile
    {
        private float ExplosionChainDistance = 10 * 16f;
        private float ExplosionSoundDistance = 14 * 16f;

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 32;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 6;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ai[0] = -1f;
        }

        public override void AI()
        {
            if (Projectile.ai[0] != -1f)
            {
                NPC target = Main.npc[(int)Projectile.ai[0]];
                if (target.active)
                {
                    Projectile.Center = target.Center + new Vector2(0f, -target.height - 25f);
                    Projectile.timeLeft = 2;
                }
                if (target.life < 0)
                {
                    Projectile.Kill();
                }
                Lighting.AddLight(Projectile.Center, 1.73f / 1.5f, 2.24f / 1.5f, 2.30f / 1.5f);
                if (Main.rand.Next(1, 5) == 1)
                    Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch)].noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Main.dedServ)
                return;

            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (npc.Distance(Projectile.position) <= ExplosionChainDistance && npc.lifeMax > 5 && !npc.friendly && !npc.immortal && !npc.hide)
                        npc.StrikeNPC(78, 5f, Projectile.direction);
                }
            }
            for (int d = 0; d < 15; d++)
            {
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Electric, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            }
            SoundStyle explosionSFX = SoundID.Item62;
            float volume = 1f - (Vector2.Distance(Main.player[Projectile.owner].position, Projectile.Center) / ExplosionSoundDistance);
            volume = Math.Clamp(volume, 0f, 1f);
            explosionSFX.Volume = volume;
            SoundEngine.PlaySound(explosionSFX, Projectile.Center);
            /*Vector2 positionDifference = Main.player[Projectile.owner].position - Projectile.Center;
            float pan = -(positionDifference.X / Math.Abs(positionDifference.X)) * volume;
            explosionSFX.Pan = pan;
            explosionSFX.*/
        }
    }
}