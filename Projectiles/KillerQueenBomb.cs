using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class KillerQueenBomb : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Extras/Bomb"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        private const float ExplosionRadius = 5.5f * 16f;

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

            for (int i = 0; i < 25; i++)        //Extra explosion effects
            {
                float angle = (360f / 25f) * i;
                Vector2 dustPosition = projectile.Center + (angle.ToRotationVector2() * 7f);
                Vector2 dustVelocity = dustPosition - projectile.Center;
                dustVelocity.Normalize();
                dustVelocity *= 7f;
                int dustIndex = Dust.NewDust(dustPosition, projectile.width, projectile.height, DustID.Fire, dustVelocity.X, dustVelocity.Y, 100, Scale: 3.5f);
                Main.dust[dustIndex].noGravity = true;
            }

            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (npc.lifeMax > 5 && !npc.friendly && !npc.hide && !npc.immortal && Vector2.Distance(projectile.Center, npc.Center) <= ExplosionRadius)
                    {
                        int hitDirection = -1;
                        if (npc.position.X - projectile.position.X > 0)
                        {
                            hitDirection = 1;
                        }
                        npc.StrikeNPC((int)projectile.ai[0], 7f, hitDirection);
                    }
                }
            }
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];
                if (player.active && !player.dead)
                {
                    if (Vector2.Distance(projectile.Center, player.Center) > ExplosionRadius)
                        continue;

                    int bombDamage = (int)projectile.ai[0];
                    if (p == player.whoAmI || Main.player[projectile.owner].team == player.team)
                    {
                        bombDamage = (int)(projectile.ai[0] * 0.25f);
                    }
                    int hitDirection = -1;
                    if (player.position.X - projectile.position.X > 0)
                    {
                        hitDirection = 1;
                    }
                    player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " got careless around one of KQ's bombs."), bombDamage, hitDirection);
                }
            }
            Main.PlaySound(SoundID.Item62);
        }
    }
}