using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.Minions
{
    public class GEButterfly : ModProjectile
    {
        public override string Texture { get { return "Terraria/Images/NPC_" + NPCID.Butterfly; } }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 24;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 45;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.netImportant = true;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.hostile = false;
        }

        private const float DetectionDistance = 16f * 16f;

        private NPC npcTarget = null;
        private bool goingUp = false;
        private bool spawnEffectsPlayed = false;

        public override void AI()
        {
            Projectile.spriteDirection = -Projectile.direction;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.lifeMax > 5 && npc.type != NPCID.TargetDummy && npc.type != NPCID.CultistTablet)
                {
                    float distance = Projectile.Distance(npc.Center);
                    if (distance < DetectionDistance)
                    {
                        npcTarget = npc;
                        Projectile.ai[0] = 1f;
                    }
                    else
                    {
                        Projectile.ai[0] = 0f;
                    }
                }
            }
            if (Projectile.ai[0] == 0f)
            {
                if (Projectile.velocity.Y <= -0.3f)
                {
                    goingUp = false;
                }
                if (Projectile.velocity.Y >= 0.3f)
                {
                    goingUp = true;
                }
                if (!goingUp)
                {
                    Projectile.velocity.Y += 0.05f;
                }
                if (goingUp)
                {
                    Projectile.velocity.Y -= 0.05f;
                }
            }
            if (Projectile.ai[0] == 1f)
            {
                if (Projectile.velocity.X > 3f)
                {
                    Projectile.velocity.X = 3f;
                }
                if (Projectile.velocity.X < -3f)
                {
                    Projectile.velocity.X = -3f;
                }
                if (Projectile.position.Y < npcTarget.position.Y)     //if it's higher   
                {
                    Projectile.velocity.Y = 1.5f;
                }
                if (Projectile.position.Y > npcTarget.position.Y)     //if it's higher   
                {
                    Projectile.velocity.Y = -1.5f;
                }
                if (npcTarget.position.X - 10f > Projectile.position.X)
                {
                    Projectile.velocity.X = 1.5f;
                    Projectile.direction = 1;
                }
                if (npcTarget.position.X + 10f < Projectile.position.X)
                {
                    Projectile.velocity.X = -1.5f;
                    Projectile.direction = -1;
                }
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 12)
            {
                Projectile.frame = 9;
            }
            if (Projectile.frame <= 8)
            {
                Projectile.frame = 9;
            }

            if (!spawnEffectsPlayed)
            {
                for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IchorTorch, Main.rand.NextFloat(-1.1f, 1.1f + 1f), Main.rand.NextFloat(-1.1f, 1.1f + 1f), Scale: Main.rand.NextFloat(1.1f, 2.4f + 1f));
                    Main.dust[dustIndex].noGravity = true;
                }
                spawnEffectsPlayed = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 5 + 1); i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, Main.rand.NextFloat(-0.3f, 1f + 0.3f), Main.rand.NextFloat(-0.3f, 0.3f + 1f), Scale: Main.rand.NextFloat(-1f, 1f + 1f));
            }
            SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.position);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.GetGlobalNPC<JoJoGlobalNPC>().taggedByButterfly = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}