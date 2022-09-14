using JoJoStands.Buffs.Debuffs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace JoJoStands.Projectiles
{
    public class BombBubble : ModProjectile

    {
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 200;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.25f;
        }
                private const float ExplosionRadius = 6f * 16f;
        private bool crit = false;

       
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Main.rand.Next(0, 1 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
            if (mPlayer.awakenedAmuletEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 80)
                    target.AddBuff(ModContent.BuffType<Infected>(), 60 * 9);
            }
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 60)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, Alpha: 100, Scale: 1.2f);
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
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
                crit = true;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (npc.lifeMax > 5 && !npc.friendly && !npc.hide && !npc.immortal && npc.Distance(Projectile.Center) <= ExplosionRadius)
                    {
                        int hitDirection = -1;
                        if (npc.position.X - Projectile.position.X > 0)
                            hitDirection = 1;

                        npc.StrikeNPC(300, 8f, hitDirection, crit);
                    }
                }
            }
            SoundEngine.PlaySound(SoundID.Item14);
            }

        }
    }