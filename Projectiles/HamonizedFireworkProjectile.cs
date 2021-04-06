using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HamonizedFireworkProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 18;
            projectile.aiStyle = 0;
            projectile.timeLeft = 100;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        private int[] explosionDustTypes = new int[12] { 200, 201, 202, 203, 204, 205, 206, 207, 211, 212, 217, 218 };

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            if (projectile.timeLeft <= 15)
            {
                projectile.velocity.X += Main.rand.NextFloat(-0.1f, 0.1f + 1f);
                projectile.velocity.Y += Main.rand.NextFloat(-0.1f, 0.1f + 1f);
            }

            if (projectile.timeLeft % 4 == 0)
            {
                int dustIndex = Dust.NewDust(projectile.Center, projectile.width, projectile.height, 169);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            int dustType = explosionDustTypes[Main.rand.Next(0, explosionDustTypes.Length)];
            for (int i = 0; i < 60; i++)
            {
                float circlePos = i;
                Vector2 spawnPos = projectile.Center + (circlePos.ToRotationVector2() * 50f);
                Vector2 velocity = spawnPos - projectile.Center;
                velocity.Normalize();
                Dust dustIndex = Dust.NewDustPerfect(spawnPos, dustType, velocity * 4f, Scale: Main.rand.NextFloat(0.8f, 2.2f));
                dustIndex.noGravity = true;
            }
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    float npcDistance = Vector2.Distance(npc.Center, projectile.Center);
                    if (npcDistance < 50f && !npc.immortal && !npc.hide && !npc.friendly)
                    {
                        npc.StrikeNPC(projectile.damage, projectile.knockBack * 5f, projectile.direction);
                        npc.AddBuff(mod.BuffType("Sunburn"), 15 * 60);
                    }
                }
            }
            Main.PlaySound(SoundID.Item62);
        }
    }
}