using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class BadCompanyBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            Projectile.velocity.Y += 0.3f;
        }

        private const float ExplosionRadius = 6f * 16f;
        private bool crit = false;

        public override void Kill(int timeLeft)
        {
            //Normal grenade explosion effects
            for (int i = 0; i < 30; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Alpha: 100, Scale: 1.5f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Alpha: 100, Scale: 3.5f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 7f;
                dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Alpha: 100, Scale: 1.5f);
                Main.dust[dustIndex].velocity *= 3f;
            }
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (npc.lifeMax > 5 && !npc.friendly && !npc.hide && !npc.immortal && npc.Distance(Projectile.Center) <= ExplosionRadius)
                    {
                        int hitDirection = -1;
                        if (npc.position.X - Projectile.position.X > 0)
                        {
                            hitDirection = 1;
                        }
                        npc.StrikeNPC(Projectile.damage, 7f, hitDirection, crit);
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
				int num733 = Gore.NewGore(new Vector2(Projectile.position.X, Projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num733].velocity *= scaleFactor9;
				Gore expr_17274_cp_0 = Main.gore[num733];
				expr_17274_cp_0.velocity.X = expr_17274_cp_0.velocity.X + 1f;
				Gore expr_17294_cp_0 = Main.gore[num733];
				expr_17294_cp_0.velocity.Y = expr_17294_cp_0.velocity.Y + 1f;
				num733 = Gore.NewGore(new Vector2(Projectile.position.X, Projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num733].velocity *= scaleFactor9;
				Gore expr_17317_cp_0 = Main.gore[num733];
				expr_17317_cp_0.velocity.X = expr_17317_cp_0.velocity.X - 1f;
				Gore expr_17337_cp_0 = Main.gore[num733];
				expr_17337_cp_0.velocity.Y = expr_17337_cp_0.velocity.Y + 1f;
				num733 = Gore.NewGore(new Vector2(Projectile.position.X, Projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num733].velocity *= scaleFactor9;
				Gore expr_173BA_cp_0 = Main.gore[num733];
				expr_173BA_cp_0.velocity.X = expr_173BA_cp_0.velocity.X + 1f;
				Gore expr_173DA_cp_0 = Main.gore[num733];
				expr_173DA_cp_0.velocity.Y = expr_173DA_cp_0.velocity.Y - 1f;
				num733 = Gore.NewGore(new Vector2(Projectile.position.X, Projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num733].velocity *= scaleFactor9;
				Gore expr_1745D_cp_0 = Main.gore[num733];
				expr_1745D_cp_0.velocity.X = expr_1745D_cp_0.velocity.X - 1f;
				Gore expr_1747D_cp_0 = Main.gore[num733];
				expr_1747D_cp_0.velocity.Y = expr_1747D_cp_0.velocity.Y - 1f;
			}*/

            SoundEngine.PlaySound(SoundID.Item62);
        }
    }
}