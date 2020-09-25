using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class PhantomMarker : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 0;
            projectile.timeLeft = 6;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.ai[0] = -1f;
        }

        public override void AI()
        {
            if (projectile.ai[0] != -1f)
            {
                NPC target = Main.npc[(int)projectile.ai[0]];
                if (target.active)
                {
                    projectile.position = target.Center + new Vector2(0f, -target.height - 25f);
                    projectile.timeLeft = 2;
                }
                if (target.life < 0)
                {
                    projectile.Kill();
                }
                Lighting.AddLight(projectile.Center, 1.73f / 1.5f, 2.24f / 1.5f, 2.30f / 1.5f);
                if (Main.rand.Next(1, 5) == 1)
                    Main.dust[Dust.NewDust(projectile.Center, projectile.width, projectile.height, 59)].noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (npc.Distance(projectile.position) <= 10 * 16 && npc.lifeMax > 5 && !npc.friendly)
                    {
                        npc.StrikeNPC(78, 5f, projectile.direction);
                    }
                }
            }
            for (int d = 0; d < 15; d++)
            {
                Dust.NewDust(projectile.Center, projectile.width, projectile.height, 226, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            }
            Main.PlaySound(SoundID.Item62);
        }
    }
}