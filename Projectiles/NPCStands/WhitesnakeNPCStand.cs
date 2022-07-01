using JoJoStands.NPCs.TownNPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.NPCStands
{
    public class WhitesnakeNPCStand : ModProjectile
    {
        public static bool whitesnakeActive = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 40;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.npcProj = true;
        }

        private const float MaximumDetectionDistance = 10f * 16f;

        private int shootTime = 30;
        private float shootSpeed = 9f;     //how fast the Projectile the minion shoots goes
        private bool normalFrames = false;
        private bool attackFrames = false;
        private float shootCount = 0f;

        public override void AI()
        {
            NPC pucci = null;
            if (Priest.userIsAlive)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.type == ModContent.NPCType<Priest>())
                    {
                        pucci = npc;
                        break;
                    }
                }
            }
            if (shootCount > 0)
                shootCount--;
            whitesnakeActive = Projectile.active;
            if (!Priest.userIsAlive)
                Projectile.Kill();
            if (Projectile.timeLeft < 256)
                Projectile.alpha = -Projectile.timeLeft + 255;

            if (pucci != null)
            {
                Vector2 vector131 = pucci.Center;
                vector131.X -= (float)((5 + pucci.width / 2) * pucci.direction);
                vector131.Y -= 15f;
                Projectile.Center = Vector2.Lerp(Projectile.Center, vector131, 0.2f);
                Projectile.velocity *= 0.8f;
                Projectile.direction = (Projectile.spriteDirection = pucci.direction);
            }

            NPC target = null;
            for (int n = 0; n < Main.maxNPCs; n++)
            {
                normalFrames = true;
                NPC npc = Main.npc[n];
                if (npc.CanBeChasedBy(this, false))
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
            SelectFrame();
            attackFrames = target != null;

            if (target == null)
                return;

            if (shootCount <= 0 && Projectile.frame == 4)
            {
                shootCount += shootTime;

                Projectile.direction = 1;
                if (target.position.X - Projectile.position.X < 0f)
                {
                    Projectile.spriteDirection = Projectile.direction = -1;
                }
                Projectile.spriteDirection = Projectile.direction;
                Vector2 shootVel = target.position - Projectile.Center;
                if (shootVel == Vector2.Zero)
                {
                    shootVel = new Vector2(0f, 1f);
                }
                shootVel.Normalize();
                shootVel *= shootSpeed;
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Disc>(), 40, 4f);
                Main.projectile[proj].npcProj = true;
                Main.projectile[proj].netUpdate = true;
                Projectile.netUpdate = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            whitesnakeActive = false;
        }

        public virtual void SelectFrame()
        {
            Projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                if (Projectile.frameCounter >= 10)
                {
                    Projectile.frame += 1;
                    Projectile.frameCounter = 0;
                    if (Projectile.frame >= 8)
                    {
                        Projectile.frame = 0;
                    }
                }
            }
            if (normalFrames)
            {
                attackFrames = false;
                Projectile.frame = 1;
            }
        }
    }
}