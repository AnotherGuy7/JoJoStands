using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HamonizedFireworkProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 100;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private int[] explosionDustTypes = new int[12] { 200, 201, 202, 203, 204, 205, 206, 207, 211, 212, 217, 218 };

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            if (Projectile.timeLeft <= 15)
            {
                Projectile.velocity.X += Main.rand.NextFloat(-0.1f, 0.1f + 1f);
                Projectile.velocity.Y += Main.rand.NextFloat(-0.1f, 0.1f + 1f);
            }

            if (Projectile.timeLeft % 4 == 0)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            int dustType = explosionDustTypes[Main.rand.Next(0, explosionDustTypes.Length)];
            for (int i = 0; i < 60; i++)
            {
                float circlePos = i;
                Vector2 spawnPos = Projectile.Center + (circlePos.ToRotationVector2() * 50f);
                Vector2 velocity = spawnPos - Projectile.Center;
                velocity.Normalize();
                Dust dustIndex = Dust.NewDustPerfect(spawnPos, dustType, velocity * 4f, Scale: Main.rand.NextFloat(0.8f, 2.2f));
                dustIndex.noGravity = true;
            }
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    float npcDistance = Vector2.Distance(npc.Center, Projectile.Center);
                    if (npcDistance < 50f && !npc.immortal && !npc.hide && !npc.friendly)
                    {
                        npc.StrikeNPC(Projectile.damage, Projectile.knockBack * 5f, Projectile.direction);
                        npc.AddBuff(ModContent.BuffType<Sunburn>(), 15 * 60);
                    }
                }
            }
            SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 62, 1f, 1.9f);
        }
    }
}