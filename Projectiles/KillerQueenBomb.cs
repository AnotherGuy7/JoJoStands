using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class KillerQueenBomb : ModProjectile
    {
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

        private const float ExplosionRadius = 6f * 16f;

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
            for (int i = 0; i < 25; i++)
            {
                float angle = (360f / 25f) * i;
                Vector2 dustVelocity = projectile.Center - (projectile.Center + (angle.ToRotationVector2() * 7f));
                dustVelocity.Normalize();
                dustVelocity *= 5f;
                int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, dustVelocity.X, dustVelocity.Y, 100, Scale: 3.5f);
                Main.dust[dustIndex].noGravity = true;
            }
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (npc.lifeMax > 5 && !npc.friendly && !npc.hide && !npc.immortal && npc.Distance(projectile.Center) <= ExplosionRadius)
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
                if (player.active)
                {
                    if (!player.dead)
                    {
                        int bombDamage = (int)projectile.ai[0];
                        if (p == player.whoAmI || Main.player[projectile.owner].team == player.team)
                        {
                            bombDamage = (int)(projectile.ai[0] * 0.75f);
                        }
                        int hitDirection = -1;
                        if (player.position.X - projectile.position.X > 0)
                        {
                            hitDirection = 1;
                        }
                        player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " got careless around one of KQ's bombs."), bombDamage, hitDirection);
                    }
                }
            }
            /*for (int i = 0; i < 2; i++)
			{
				float scaleFactor9 = 0.4f;
				if (i == 1)
				{
					scaleFactor9 = 0.8f;
				}
				int num733 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num733].velocity *= scaleFactor9;
				Gore expr_17274_cp_0 = Main.gore[num733];
				expr_17274_cp_0.velocity.X = expr_17274_cp_0.velocity.X + 1f;
				Gore expr_17294_cp_0 = Main.gore[num733];
				expr_17294_cp_0.velocity.Y = expr_17294_cp_0.velocity.Y + 1f;
				num733 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num733].velocity *= scaleFactor9;
				Gore expr_17317_cp_0 = Main.gore[num733];
				expr_17317_cp_0.velocity.X = expr_17317_cp_0.velocity.X - 1f;
				Gore expr_17337_cp_0 = Main.gore[num733];
				expr_17337_cp_0.velocity.Y = expr_17337_cp_0.velocity.Y + 1f;
				num733 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num733].velocity *= scaleFactor9;
				Gore expr_173BA_cp_0 = Main.gore[num733];
				expr_173BA_cp_0.velocity.X = expr_173BA_cp_0.velocity.X + 1f;
				Gore expr_173DA_cp_0 = Main.gore[num733];
				expr_173DA_cp_0.velocity.Y = expr_173DA_cp_0.velocity.Y - 1f;
				num733 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num733].velocity *= scaleFactor9;
				Gore expr_1745D_cp_0 = Main.gore[num733];
				expr_1745D_cp_0.velocity.X = expr_1745D_cp_0.velocity.X - 1f;
				Gore expr_1747D_cp_0 = Main.gore[num733];
				expr_1747D_cp_0.velocity.Y = expr_1747D_cp_0.velocity.Y - 1f;
			}*/

            Main.PlaySound(SoundID.Item62);
        }
    }
}