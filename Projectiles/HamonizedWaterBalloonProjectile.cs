using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class HamonizedWaterBalloonProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1800;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        //Projectile.ai[0] == 0f means that it's meant to be thrown and explode on impact
        //Projectile.ai[0] == 1f means that it's meant to be used as a trap and explodes on enemy impact only

        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(270f);

            if (Projectile.timeLeft % 3 == 0)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 169);
                Main.dust[dustIndex].noGravity = true;
            }

            if (Projectile.velocity.Y < 6f)
            {
                Projectile.velocity.Y += 0.2f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X = 0f;
            return Projectile.ai[0] == 0f;      //If the balloon is set to explode on impact, then it's true, if it's not, then, well, it's not.
        }

        public override void Kill(int timeLeft)
        {
            float radius = 5f * 16f;
            for (int i = 0; i < 60; i++)
            {
                float circlePos = i;
                Vector2 spawnPos = Projectile.Center + (circlePos.ToRotationVector2() * radius);
                Vector2 velocity = spawnPos - Projectile.Center;
                velocity.Normalize();
                Dust dustIndex = Dust.NewDustPerfect(spawnPos, 169, velocity * 4f, Scale: Main.rand.NextFloat(0.8f, 2.2f));
                dustIndex.noGravity = true;

                Vector2 randomDustPos = Projectile.Center + new Vector2(Main.rand.NextFloat(-radius, radius + 1f), Main.rand.NextFloat(-radius, radius + 1f));
                Dust randomDustIndex = Dust.NewDustPerfect(randomDustPos, 169, velocity * 4f, Scale: Main.rand.NextFloat(0.8f, 2.2f));
                randomDustIndex.noGravity = true;
            }
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active)
                {
                    float npcDistance = Vector2.Distance(npc.Center, Projectile.Center);
                    if (npcDistance < radius && !npc.immortal && !npc.hide && !npc.friendly)
                    {
                        npc.StrikeNPC(Projectile.damage, Projectile.knockBack * 5f, Projectile.direction);
                        npc.AddBuff(ModContent.BuffType<Sunburn>(), 25 * 60);
                    }
                }
            }
            SoundStyle tinkSound = SoundID.Tink;
            tinkSound.Pitch = 2.5f;
            SoundEngine.PlaySound(tinkSound, Projectile.Center);
        }
    }
}