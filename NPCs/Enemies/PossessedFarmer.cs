using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Items;
using JoJoStands.Projectiles;

namespace JoJoStands.NPCs.Enemies
{
    public class PossessedFarmer : ModNPC
    {
        private enum State { Roam = 0, Windup = 1, Shoot = 2, Lunge = 3 }

        private const float PreferredDist = 240f;
        private const float TooCloseDist = 150f;
        private const float ShootRange = 350f;
        private const float LungeRange = 200f;

        private const int WindupDuration = 30;
        private const int ShotsPerBurst = 3;
        private const int FramePerShot = 8;
        private const int RoamCooldown = 120;
        private const int LungeDuration = 28;
        private const float LungeSpeed = 11f;

        private ref float StateTimer => ref NPC.ai[0];
        private ref float CurrentState => ref NPC.ai[1];
        private ref float ShotsFired => ref NPC.ai[2];
        private ref float ShotTimer => ref NPC.ai[3];

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 60;
            NPC.damage = 30;
            NPC.defense = 20;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 500f;
            NPC.knockBackResist = 0.25f;
            NPC.aiStyle = 0;
            NPC.friendly = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
        }

        public override void AI()
        {
            // ── gravity ──────────────────────────────────────────────
            if (!NPC.noGravity)
                NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y + 0.4f, -20f, 16f);

            NPC.TargetClosest(true);
            Player target = Main.player[NPC.target];
            if (!target.active || target.dead) return;

            Vector2 toPlayer = target.Center - NPC.Center;
            float dist = toPlayer.Length();
            Vector2 dirToPlayer = dist > 0.001f ? Vector2.Normalize(toPlayer) : Vector2.UnitX;

            NPC.spriteDirection = dirToPlayer.X >= 0 ? 1 : -1;
            StateTimer++;

            switch ((State)(int)CurrentState)
            {
                case State.Roam: DoRoam(dist, dirToPlayer); break;
                case State.Windup: DoWindup(dist, dirToPlayer); break;
                case State.Shoot: DoShoot(dirToPlayer); break;
                case State.Lunge: DoLunge(); break;
            }
        }

        private void DoRoam(float dist, Vector2 dirToPlayer)
        {
            if (dist < TooCloseDist)
                NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, -dirToPlayer.X * 3f, 0.15f);
            else if (dist > PreferredDist)
                NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, dirToPlayer.X * 2.5f, 0.13f);
            else
                NPC.velocity.X *= 0.88f;

            if (NPC.collideX)
                NPC.velocity.Y = -5f;

            if (StateTimer < RoamCooldown) return;

            if (dist <= LungeRange)
            {
                TransitionTo(State.Lunge);
                NPC.localAI[0] = dirToPlayer.X;
            }
            else if (dist <= ShootRange)
            {
                TransitionTo(State.Windup);
            }
        }

        private void DoWindup(float dist, Vector2 dirToPlayer)
        {
            NPC.velocity.X *= 0.82f;
            if (dist > ShootRange * 1.3f)
            {
                TransitionTo(State.Roam);
                return;
            }
            if (dist <= LungeRange)
            {
                TransitionTo(State.Lunge);
                NPC.localAI[0] = dirToPlayer.X;
                return;
            }

            if (StateTimer >= WindupDuration)
            {
                ShotsFired = 0;
                ShotTimer = FramePerShot;
                TransitionTo(State.Shoot);
            }
        }

        private void DoShoot(Vector2 dirToPlayer)
        {
            NPC.velocity.X *= 0.85f;
            ShotTimer++;

            if (ShotTimer < FramePerShot) return;
            ShotTimer = 0;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                float spread = MathHelper.ToRadians(Main.rand.NextFloat(-5f, 5f));
                Vector2 shootDir = dirToPlayer.RotatedBy(spread);

                Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    NPC.Center,
                    shootDir * 11f,
                    ModContent.ProjectileType<AnubisBladeNPCProjectile>(),
                    40,
                    3f,
                    Main.myPlayer
                );
            }

            ShotsFired++;
            NPC.netUpdate = true;

            if (ShotsFired >= ShotsPerBurst)
                TransitionTo(State.Roam);
        }

        private void DoLunge()
        {
            if (StateTimer == 1)
            {
                float lungeX = NPC.localAI[0] >= 0 ? 1f : -1f;
                NPC.velocity.X = lungeX * LungeSpeed;
                NPC.velocity.Y = -3.5f;
                NPC.netUpdate = true;
            }

            NPC.velocity.X *= 0.94f;

            if (StateTimer >= LungeDuration)
                TransitionTo(State.Roam);
        }

        private void TransitionTo(State next)
        {
            CurrentState = (float)next;
            StateTimer = 0;
            NPC.netUpdate = true;
        }

        public override void FindFrame(int frameHeight)
        {
            bool moving = System.Math.Abs(NPC.velocity.X) > 0.5f;
            if (!moving) return;

            NPC.frameCounter += System.Math.Abs(NPC.velocity.X) * 0.45f;
            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= 14 * frameHeight)
                    NPC.frame.Y = 0;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.dayTime && spawnInfo.Player.ZoneDesert && NPC.downedPlantBoss)
                return 0.08f;
            return 0f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 15; i++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
            }
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(),
                ModContent.ItemType<AnubisBladeItem>(), 1);
        }
    }
}