using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using JoJoStands.NPCs.TownNPCs;

namespace JoJoStands.Projectiles.NPCStands
{
    public class StarPlatinumNPCStand : ModProjectile
    {
        public static bool SPActive = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.LightPet[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
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
            if (MarineBiologist.userIsAlive)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.type == Mod.NPCType("MarineBiologist>())
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
                vector131.X -= (float)((15 + jotaro.width / 2) * jotaro.direction);
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
            if (Projectile.ai[1] > 0f)
            {
                Projectile.ai[1] += 1f;
                if (Main.rand.Next(3) == 0)
                {
                    Projectile.ai[1] += 1f;
                }
            }
            if (Projectile.ai[1] > shootCool)
            {
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
            }

            if (target == null)
                return;

            if (Projectile.ai[0] == 0f)
            {                    
                attackFrames = true;
                normalFrames = false;
                Projectile.direction = 1;
                if (target.position.X - Projectile.Center.X < 0f)
                {
                    Projectile.spriteDirection = (Projectile.direction = -1);
                }
                Projectile.spriteDirection = Projectile.direction;
                if (Projectile.ai[1] == 0f)
                {
                    Projectile.ai[1] = 1f;
                    Vector2 shootVel = target.position - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<NPCStandFists>(), MarineBiologist.standDamage, 1f, Main.myPlayer, 0, 0);
                    Main.projectile[proj].npcProj = true;
                    Main.projectile[proj].netUpdate = true;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                attackFrames = false;
            }
        }

        public override void Kill(int timeLeft)
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
                    {
                        Projectile.frame = 2;
                    }
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
                {
                    Projectile.frame = 0;
                }
            }
        }
    }
}