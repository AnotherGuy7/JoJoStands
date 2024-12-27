using JoJoStands.NPCs.TownNPCs;
using JoJoStands.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.NPCStands
{
    public class StarPlatinumNPCStand : ModProjectile
    {
        public static bool SPActive = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 40;
            Projectile.height = 74;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.npcProj = true;
        }

        private const float MaximumDetectionDistance = 6f * 16f;

        private float shootCool = 6f;       //how fast the minion can shoot
        private float shootSpeed = 9f;     //how fast the Projectile the minion shoots goes
        private bool normalFrames = false;
        private bool attackFrames = false;

        public override void AI()
        {
            NPC jotaro = null;
            if (MarineBiologist.UserIsAlive)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.type == ModContent.NPCType<MarineBiologist>())
                    {
                        jotaro = npc;
                        break;
                    }
                }
            }
            else
            {
                Projectile.Kill();
            }

            SPActive = Projectile.active;
            Projectile.tileCollide = true;
            if (Projectile.spriteDirection == 1)
            {
                DrawOffsetX = -10;
            }
            if (Projectile.spriteDirection == -1)
            {
                DrawOffsetX = -60;
            }
            if (jotaro != null)
            {
                Vector2 vector131 = jotaro.Center;
                vector131.X -= (float)((8 + jotaro.width / 2) * jotaro.direction);
                vector131.Y -= 15f;
                Projectile.Center = Vector2.Lerp(Projectile.Center, vector131, 0.2f);
                Projectile.velocity *= 0.8f;
                Projectile.direction = (Projectile.spriteDirection = jotaro.direction);
            }
            if (Projectile.timeLeft < 256)
            {
                Projectile.alpha = -Projectile.timeLeft + 255;
            }

            NPC target = null;
            normalFrames = true;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                NPC npc = Main.npc[n];
                if (npc.active && npc.CanBeChasedBy(this))
                {
                    float distance = Vector2.Distance(npc.Center, Projectile.Center);
                    if (distance < MaximumDetectionDistance && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                    {
                        target = npc;
                        Projectile.alpha = 0;
                        Projectile.timeLeft = 600;
                    }
                }
            }

            if (Projectile.ai[1] > 0f)
            {
                Projectile.ai[1] += 1f;
                if (Main.rand.Next(0, 3 + 1) == 0)
                    Projectile.ai[1] += 1f;
            }
            if (Projectile.ai[1] > shootCool)
            {
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
            }

            if (target != null)
            {
                attackFrames = true;
                normalFrames = false;
                Projectile.direction = 1;
                if (target.position.X - Projectile.Center.X < 0f)
                    Projectile.direction = -1;

                Projectile.spriteDirection = Projectile.direction;
                if (Projectile.ai[1] == 0f)
                {
                    Projectile.ai[1] = 1f;
                    Vector2 shootVel = target.Center - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);

                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<NPCStandFists>(), MarineBiologist.standDamage, 1f, Main.myPlayer, 0, 0);
                    Main.projectile[projIndex].npcProj = true;
                    Main.projectile[projIndex].netUpdate = true;
                    Projectile.netUpdate = true;
                }
            }
            else
                attackFrames = false;

            SelectFrame();
        }

        public override void OnKill(int timeLeft)
        {
            SPActive = false;
        }

        public virtual void SelectFrame()
        {
            Projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                if (Projectile.frameCounter >= 6)
                {
                    Projectile.frame += 1;
                    Projectile.frameCounter = 0;
                    if (Projectile.frame >= 4)
                        Projectile.frame = 2;
                }
            }
            if (normalFrames)
            {
                attackFrames = false;
                if (Projectile.frameCounter >= 14)
                {
                    Projectile.frame += 1;
                    Projectile.frameCounter = 0;
                }
                if (Projectile.frame >= 2)
                    Projectile.frame = 0;
            }
        }
    }
}