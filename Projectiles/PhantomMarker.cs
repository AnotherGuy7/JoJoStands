using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class PhantomMarker : ModProjectile
    {
        private const float ExplosionChainDistance = 16 * 16f;
        private const float ExplosionSoundDistance = 20 * 16f;

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
                    Projectile.Kill();

                Lighting.AddLight(Projectile.Center, 1.73f / 1.5f, 2.24f / 1.5f, 2.30f / 1.5f);
                if (Main.rand.Next(1, 5) == 1)
                    Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch)].noGravity = true;
            }
            else
                Projectile.Kill();
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
                    if (npc.Distance(Projectile.Center) <= ExplosionChainDistance && npc.lifeMax > 5 && !npc.friendly && !npc.immortal && !npc.hide)
                    {
                        NPC.HitInfo hitInfo = new NPC.HitInfo()
                        {
                            Damage = 135,
                            Knockback = 5f,
                            HitDirection = Projectile.direction
                        };
                        npc.StrikeNPC(hitInfo);
                        for (int d = 0; d < 40; d++)
                        {
                            Vector2 dustPosition = Vector2.Lerp(Main.npc[(int)Projectile.ai[0]].Center, npc.Center, d * (1 / 40f));
                            int dustIndex = Dust.NewDust(dustPosition, 4, 4, DustID.BlueTorch, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                            Main.dust[dustIndex].noGravity = true;
                            Main.dust[dustIndex].noLight = true;
                        }
                    }

                }
            }
            for (int d = 0; d < 15; d++)
            {
                int dustIndex = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Electric, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].noLight = true;
            }
            SoundStyle explosionSFX = SoundID.Item62;
            float volume = 1f - (Vector2.Distance(Main.player[Projectile.owner].position, Projectile.Center) / ExplosionSoundDistance);
            volume = Math.Clamp(volume, 0f, 1f);
            explosionSFX.Volume = volume;
            SoundEngine.PlaySound(explosionSFX, Projectile.Center);
        }
    }
}