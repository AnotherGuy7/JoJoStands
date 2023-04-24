using JoJoStands.Projectiles.PlayerStands.KillerQueenBTD;
using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ExplosiveBubble : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private const float ExplosionRadius = 6f * 16f;
        private bool crit = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile ownerProj = Main.projectile[(int)Projectile.ai[1]];
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.VilePowder, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
            if (Main.mouseRight && Projectile.owner == Main.myPlayer && Projectile.ai[0] == 1f && player.ownedProjectileCounts[ModContent.ProjectileType<KillerQueenBTDStand>()] == 1 && ownerProj.ai[0] == 1f)
            {
                Projectile.damage *= 2;
                Projectile.Kill();
            }

            if (JoJoStands.AutomaticActivations)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        float npcDistance = Vector2.Distance(npc.Center, Projectile.Center);
                        if (npcDistance < ExplosionRadius - 16f && !npc.immortal && !npc.hide && !npc.friendly && npc.lifeMax > 5)
                        {
                            Projectile.Kill();
                            break;
                        }
                    }
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.Next(1, 100 + 1) >= 60)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
            }
            damage = 0;
        }

        public override void Kill(int timeLeft)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
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

            for (int i = 0; i < 25; i++)        //Extra explosion effects
            {
                float angle = (360f / 25f) * i;
                Vector2 dustPosition = Projectile.Center + (angle.ToRotationVector2() * 7f);
                Vector2 dustVelocity = dustPosition - Projectile.Center;
                dustVelocity.Normalize();
                dustVelocity *= 7f;
                int dustIndex = Dust.NewDust(dustPosition, Projectile.width, Projectile.height, DustID.Torch, dustVelocity.X, dustVelocity.Y, 100, Scale: 3.5f);
                Main.dust[dustIndex].noGravity = true;
            }

            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    if (npc.lifeMax > 5 && !npc.friendly && !npc.hide && !npc.immortal && Vector2.Distance(Projectile.Center, npc.Center) <= ExplosionRadius)
                    {
                        int hitDirection = -1;
                        if (npc.position.X - Projectile.position.X > 0)
                            hitDirection = 1;

                        npc.StrikeNPC(Projectile.damage, 7f, hitDirection, crit);
                    }
                }
            }
            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];
                if (player.active && !player.dead)
                {
                    if (Vector2.Distance(Projectile.Center, player.Center) > ExplosionRadius)
                        continue;

                    int bombDamage = (int)Projectile.damage;
                    if (p == player.whoAmI || !player.InOpposingTeam(Main.player[Projectile.owner]))
                        bombDamage = (int)(Projectile.damage * 0.10f);

                    int hitDirection = -1;
                    if (player.position.X - Projectile.position.X > 0)
                        hitDirection = 1;

                    player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " got careless around one of KQ's bombs."), bombDamage, hitDirection);
                }
            }
            SoundEngine.PlaySound(SoundID.Item62);

            for (int i = 0; i < 60; i++)
            {
                float circlePos = i;
                Vector2 spawnPos = Projectile.Center + (circlePos.ToRotationVector2() * 50f);
                Vector2 velocity = spawnPos - Projectile.Center;
                velocity.Normalize();
                Dust dustIndex = Dust.NewDustPerfect(spawnPos, 21, velocity * 4f, Scale: Main.rand.NextFloat(0.8f, 2.2f));
                dustIndex.noGravity = true;
            }
        }
    }
}