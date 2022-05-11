using JoJoStands.Projectiles.PlayerStands.KillerQueenBTD;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class Bubble : ModProjectile
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

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile ownerProj = Main.projectile[(int)Projectile.ai[1]];
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 21, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
            if (Main.mouseRight && Projectile.owner == Main.myPlayer && Projectile.ai[0] == 1f && player.ownedProjectileCounts[ModContent.ProjectileType<KillerQueenBTDStand>()] == 1 && ownerProj.ai[0] == 1f)
            {
                Projectile.damage *= 2;
                Projectile.Kill();
                Projectile.netUpdate = true;
            }

            if (MyPlayer.AutomaticActivations)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active)
                    {
                        float npcDistance = Vector2.Distance(npc.Center, Projectile.Center);
                        if (npcDistance < 30f && !npc.immortal && !npc.hide && !npc.friendly && npc.lifeMax > 5)
                        {
                            Projectile.Kill();
                            break;
                        }
                    }
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
                crit = true;
            damage = 0;
        }

        public override void Kill(int timeLeft)
        {
            int explosion = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ProjectileID.GrenadeIII, Projectile.damage, 8f, Projectile.owner);
            Main.projectile[explosion].timeLeft = 2;
            Main.projectile[explosion].netUpdate = true;
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