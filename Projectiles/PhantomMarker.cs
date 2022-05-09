using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class PhantomMarker : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
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
                    Projectile.position = target.Center + new Vector2(0f, -target.height - 25f);
                    Projectile.timeLeft = 2;
                }
                if (target.life < 0)
                {
                    Projectile.Kill();
                }
                Lighting.AddLight(Projectile.Center, 1.73f / 1.5f, 2.24f / 1.5f, 2.30f / 1.5f);
                if (Main.rand.Next(1, 5) == 1)
                    Main.dust[Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, 59)].noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (npc.Distance(Projectile.position) <= 10 * 16 && npc.lifeMax > 5 && !npc.friendly && !npc.immortal && !npc.hide)
                    {
                        npc.StrikeNPC(78, 5f, Projectile.direction);
                    }
                }
            }
            for (int d = 0; d < 15; d++)
            {
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, 226, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            }
            SoundEngine.PlaySound(SoundID.Item62);
        }
    }
}