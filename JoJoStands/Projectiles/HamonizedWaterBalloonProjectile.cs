using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HamonizedWaterBalloonProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 16;
            projectile.aiStyle = 0;
            projectile.timeLeft = 1800;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        //projectile.ai[0] == 0f means that it's meant to be thrown and explode on impact
        //projectile.ai[0] == 1f means that it's meant to be used as a trap and explodes on enemy impact only

        public override void AI()
        {
            if (projectile.ai[0] == 0f)
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(270f);

            if (projectile.timeLeft % 3 == 0)
            {
                int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, 169);
                Main.dust[dustIndex].noGravity = true;
            }

            if (projectile.velocity.Y < 6f)
            {
                projectile.velocity.Y += 0.2f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity.X = 0f;
            return projectile.ai[0] == 0f;      //If the balloon is set to explode on impact, then it's true, if it's not, then, well, it's not.
        }

        public override void Kill(int timeLeft)
        {
            float radius = 5f * 16f;
            for (int i = 0; i < 60; i++)
            {
                float circlePos = i;
                Vector2 spawnPos = projectile.Center + (circlePos.ToRotationVector2() * radius);
                Vector2 velocity = spawnPos - projectile.Center;
                velocity.Normalize();
                Dust dustIndex = Dust.NewDustPerfect(spawnPos, 169, velocity * 4f, Scale: Main.rand.NextFloat(0.8f, 2.2f));
                dustIndex.noGravity = true;

                Vector2 randomDustPos = projectile.Center + new Vector2(Main.rand.NextFloat(-radius, radius + 1f), Main.rand.NextFloat(-radius, radius + 1f));
                Dust randomDustIndex = Dust.NewDustPerfect(randomDustPos, 169, velocity * 4f, Scale: Main.rand.NextFloat(0.8f, 2.2f));
                randomDustIndex.noGravity = true;
            }
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    float npcDistance = Vector2.Distance(npc.Center, projectile.Center);
                    if (npcDistance < radius && !npc.immortal && !npc.hide && !npc.friendly)
                    {
                        npc.StrikeNPC(projectile.damage, projectile.knockBack * 5f, projectile.direction);
                        npc.AddBuff(mod.BuffType("Sunburn"), 25 * 60);
                    }
                }
            }
            Main.PlaySound(0, (int)projectile.Center.X, (int)projectile.Center.Y, 1, 1f, 2.5f);
        }
    }
}