using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class BadCompanyTankRocket : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/AerosmithBomb"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            projectile.velocity.Y += 0.1f;
        }

        private const float ExplosionRadius = 4f * 16f;

        public override void Kill(int timeLeft)
        {
            //Normal grenade explosion effects
            for (int i = 0; i < 30; i++)
            {
                int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Smoke, Alpha: 100, Scale: 1.5f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, Alpha: 100, Scale: 3.5f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 7f;
                dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, Alpha: 100, Scale: 1.5f);
                Main.dust[dustIndex].velocity *= 3f;
            }

            MyPlayer mPlayer = Main.player[projectile.owner].GetModPlayer<MyPlayer>();
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (npc.lifeMax > 5 && !npc.friendly && !npc.hide && !npc.immortal && npc.Distance(projectile.Center) <= ExplosionRadius)
                    {
                        int hitDirection = -1;
                        if (npc.position.X - projectile.position.X > 0)
                            hitDirection = 1;

                        npc.StrikeNPC((int)(projectile.ai[0] * mPlayer.standDamageBoosts), 8f, hitDirection);
                    }
                }
            }

            Main.PlaySound(SoundID.Item62);
        }
    }
}