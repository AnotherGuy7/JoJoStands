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
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 60;
            NPC.damage = 0;
            NPC.defense = 20;
            NPC.lifeMax = 500;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 500f;
            NPC.knockBackResist = 0.2f;
            NPC.aiStyle = 3;
            NPC.friendly = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
        }

        private const float PreferredDistance = 220f;
        private const float TooCloseDistance = 160f;
        private const float AttackRange = 320f;
        private const int BurstWindup = 50;
        private const int ShotsPerBurst = 8;
        private const int FramesBetweenShots = 6;
        private const int BurstCooldown = 90;

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player target = Main.player[NPC.target];
            if (!target.active || target.dead)
                return;

            Vector2 toPlayer = target.Center - NPC.Center;
            float distance = toPlayer.Length();
            Vector2 dirToPlayer = distance > 0.001f ? Vector2.Normalize(toPlayer) : Vector2.UnitX;

            NPC.spriteDirection = dirToPlayer.X > 0 ? 1 : -1;

            ref float attackTimer = ref NPC.ai[0];
            ref float burstShots = ref NPC.ai[1];
            ref float burstShotTimer = ref NPC.ai[2];
            ref float state = ref NPC.ai[3];

            if (state == 0f)
            {
                NPC.aiStyle = 3;
                AIType = 3;

                if (distance < TooCloseDistance)
                {
                    NPC.direction = dirToPlayer.X > 0 ? -1 : 1;
                    NPC.velocity.X = -dirToPlayer.X * 3.5f;
                }
                else if (distance > PreferredDistance)
                {
                    NPC.direction = dirToPlayer.X > 0 ? 1 : -1;
                    NPC.velocity.X = dirToPlayer.X * 3f;
                }
                else
                {
                    NPC.velocity.X *= 0.85f;
                }

                if (distance <= AttackRange)
                {
                    attackTimer++;
                    if (attackTimer >= BurstWindup)
                    {
                        attackTimer = 0;
                        burstShots = 0;
                        burstShotTimer = 0;
                        state = 1f;
                        NPC.netUpdate = true;
                    }
                }
                else
                {
                    attackTimer = 0;
                }
            }
            else if (state == 1f)
            {
                NPC.aiStyle = 0;
                NPC.velocity.X *= 0.8f;

                burstShotTimer++;
                if (burstShotTimer >= FramesBetweenShots)
                {
                    burstShotTimer = 0;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        float spread = MathHelper.ToRadians(Main.rand.NextFloat(-6f, 6f));
                        Vector2 shootDir = dirToPlayer.RotatedBy(spread);

                        Projectile.NewProjectile(
                            NPC.GetSource_FromAI(),
                            NPC.Center,
                            shootDir * 12f,
                            ModContent.ProjectileType<AnubisBladeNPCProjectile>(),
                            45,
                            4f,
                            Main.myPlayer
                        );
                    }

                    burstShots++;
                    NPC.netUpdate = true;

                    if (burstShots >= ShotsPerBurst)
                    {
                        attackTimer = -(BurstCooldown);
                        burstShots = 0;
                        state = 0f;
                        NPC.netUpdate = true;
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            bool isMoving = System.Math.Abs(NPC.velocity.X) > 0.5f;

            if (!isMoving)
                return;

            NPC.frameCounter += System.Math.Abs(NPC.velocity.X) * 0.5f;
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
            Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ModContent.ItemType<AnubisBladeItem>(), 1);
        }
    }
}