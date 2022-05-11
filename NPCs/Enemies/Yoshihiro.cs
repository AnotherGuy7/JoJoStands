using JoJoStands.Items;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class Yoshihiro : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 12; //this defines how many frames the NPC sprite sheet has
        }

        public override void SetDefaults()
        {
            NPC.width = 38; //the NPC sprite width
            NPC.height = 32;  //the NPC sprite height
            NPC.defense = 13;  //the NPC defense
            NPC.lifeMax = 180;  // the NPC life
            NPC.HitSound = SoundID.NPCHit1;  //the NPC sound when is hit
            NPC.DeathSound = SoundID.NPCDeath1;  //the NPC sound when he dies
            NPC.knockBackResist = 2f;  //the NPC knockback resistance
            NPC.chaseable = true;       //whether or not minions can chase this NPC
            NPC.damage = 14;       //the damage the NPC does
            NPC.aiStyle = 0;        //no AI, to run void AI()
            NPC.noGravity = true;
        }

        public float targetDistance = 0f;
        public bool dashFrames = false;
        public bool throwFrames = false;
        public bool paperFrames = false;
        public int frame = 0;
        public int yosarrowWhoAmI = -1;
        public float baseSpeed = 1.8f;
        public bool hasNoArrow = false;
        public bool dashing = false;
        public bool throwingArrow = false;
        public int timesDashed = 0;
        public bool runningAway = false;
        public int tileDetectionAddition = 0;

        public override void AI()
        {
            Player target = Main.player[NPC.target];
            if (NPC.life <= NPC.lifeMax * 0.1)
            {
                runningAway = true;
                NPC.netUpdate = true;
            }
            if (NPC.position.X > target.position.X)
            {
                NPC.velocity.X = baseSpeed + 0.2f;
            }
            if (NPC.position.X <= target.position.X)
            {
                NPC.velocity.X = -baseSpeed;
            }
            if (runningAway)
            {
                if (NPC.position.X > target.position.X)
                {
                    NPC.velocity.X = baseSpeed + 0.2f;
                }
                if (NPC.position.X <= target.position.X)
                {
                    NPC.velocity.X = -baseSpeed;
                }
            }
            if (!runningAway && !hasNoArrow && !dashing)
            {
                if (NPC.Center.X < target.position.X - 3f)
                {
                    NPC.velocity.X = baseSpeed;
                }
                if (NPC.Center.X > target.position.X + 3f)
                {
                    NPC.velocity.X = -baseSpeed;
                }
                if (NPC.Center.X > target.position.X - 3f && NPC.Center.X < target.position.X + 3f)
                {
                    NPC.velocity.X = 0f;
                }
                if (NPC.Center.Y > target.position.Y + 3f)
                {
                    NPC.velocity.Y = -baseSpeed;
                }
                if (NPC.Center.Y < target.position.Y - 3f)
                {
                    NPC.velocity.Y = baseSpeed;
                }
                if (NPC.Center.Y < target.position.Y + 3f && NPC.Center.Y > target.position.Y - 3f)       //so that he keeps going until he reaches target.position.Y +- 5f
                {
                    NPC.velocity.Y = 0f;
                }
                NPC.netUpdate = true;
            }
            if (!dashing)
            {
                if (NPC.velocity.X > 0)
                {
                    NPC.direction = 1;
                    tileDetectionAddition = 3;
                }
                if (NPC.velocity.X < 0)
                {
                    NPC.direction = -1;
                    tileDetectionAddition = -1;
                }
            }
            NPC.spriteDirection = NPC.direction;
            if (NPC.ai[0] > 0f)     //an attack timer
            {
                NPC.ai[0] -= 1f;
            }
            if (NPC.ai[1] > 0f)     //dash timer
            {
                NPC.velocity.X = 15 * NPC.direction;
                NPC.ai[1] -= 1f;
            }
            if (WorldGen.SolidTile((int)(NPC.position.X / 16) + tileDetectionAddition, (int)(NPC.position.Y / 16f)) || WorldGen.SolidTile((int)(NPC.position.X / 16) + tileDetectionAddition, (int)(NPC.position.Y / 16f) + 1) || WorldGen.SolidTile((int)(NPC.position.X / 16) + tileDetectionAddition, (int)(NPC.position.Y / 16f) + 2))
            {
                NPC.ai[3] += 1f;
            }
            if (NPC.ai[3] >= 180f)
            {
                paperFrames = true;
                NPC.noTileCollide = true;
            }
            if (paperFrames)
            {
                NPC.ai[3] -= 1f;
                NPC.noTileCollide = true;
                if (NPC.ai[3] <= 0)
                {
                    NPC.noTileCollide = false;
                    paperFrames = false;
                }
                NPC.netUpdate = true;
            }
            if (NPC.ai[0] <= 0f && !hasNoArrow && !dashing && !throwingArrow && !paperFrames && !runningAway)
            {
                targetDistance = Vector2.Distance(NPC.Center, target.Center);
                if (targetDistance < 180f)
                {
                    dashing = true;
                }
                if (targetDistance >= 180f)
                {
                    throwingArrow = true;
                    throwFrames = true;
                }
            }
            if (throwingArrow)
            {
                if (frame == 7 && yosarrowWhoAmI == -1)
                {
                    NPC.ai[0] += 30f;
                    Vector2 shootVel = target.Center - NPC.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= 16f;
                    yosarrowWhoAmI = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, shootVel, ModContent.ProjectileType<Yosarrow>(), 21, 1f);
                    Main.projectile[yosarrowWhoAmI].netUpdate = true;
                    NPC.netUpdate = true;
                }
                if (!throwFrames)       //goes false at the 10th frame
                {
                    hasNoArrow = true;
                    throwingArrow = false;
                }
            }
            if (hasNoArrow)
            {
                dashFrames = false;
                if (yosarrowWhoAmI != -1)
                {
                    Projectile targetProjectile = Main.projectile[yosarrowWhoAmI];
                    targetProjectile.timeLeft++;
                    targetDistance = Vector2.Distance(NPC.Center, targetProjectile.Center);
                    if (NPC.Center.X < targetProjectile.position.X - 5f)
                    {
                        NPC.velocity.X = 5f;
                        NPC.direction = 1;
                    }
                    if (NPC.Center.X > targetProjectile.position.X + 5f)
                    {
                        NPC.velocity.X = -5f;
                        NPC.direction = -1;
                    }
                    if (NPC.Center.X > targetProjectile.position.X - 5f && NPC.Center.X < targetProjectile.position.X + 5f)
                    {
                        NPC.velocity.X = 0f;
                    }
                    if (NPC.Center.Y > targetProjectile.position.Y + 5f)
                    {
                        NPC.velocity.Y = -5f;
                    }
                    if (NPC.Center.Y < targetProjectile.position.Y - 5f)
                    {
                        NPC.velocity.Y = 5f;
                    }
                    if (NPC.Center.Y < targetProjectile.position.Y + 5f && NPC.Center.Y > targetProjectile.position.Y - 5f)
                    {
                        NPC.velocity.Y = 0f;
                    }
                    if (targetDistance < 20f)
                    {
                        Main.projectile[yosarrowWhoAmI].Kill();
                        yosarrowWhoAmI = -1;
                        NPC.ai[0] += 150f + Main.rand.Next(0, 31);
                        NPC.netUpdate = true;
                        hasNoArrow = false;
                    }
                }
                if (yosarrowWhoAmI == -1)       //in case the projectlie dies somehow
                {
                    NPC.ai[0] += 150f + Main.rand.Next(0, 31);
                    hasNoArrow = false;
                    NPC.netUpdate = true;
                }
            }
            if (dashing)
            {
                dashFrames = true;
                if (NPC.ai[1] <= 0f)
                {
                    NPC.ai[1] += 45f;
                    if (NPC.Center.X < target.Center.X)
                    {
                        timesDashed += 1;
                        NPC.ai[2] = 1f;
                    }
                    if (NPC.Center.X > target.Center.X)
                    {
                        timesDashed += 1;
                        NPC.ai[2] = -1f;
                    }
                }
                if (NPC.ai[2] == 1f)
                {
                    NPC.velocity.X = 15f;
                    NPC.velocity.Y = 0f;
                    NPC.direction = 1;
                }
                if (NPC.ai[2] == -1f)
                {
                    NPC.velocity.X = -15f;
                    NPC.velocity.Y = 0f;
                    NPC.direction = -1;
                }
                if (NPC.ai[2] != 0 && NPC.ai[1] <= 10f)
                {
                    NPC.ai[2] = 0f;
                    NPC.velocity = Vector2.Zero;
                }
                if (timesDashed >= 3)
                {
                    timesDashed = 0;
                    NPC.ai[0] += 150f + Main.rand.Next(0, 31);
                    dashFrames = false;
                    dashing = false;
                }
                NPC.netUpdate = true;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(dashing);
            writer.Write(throwingArrow);
            writer.Write(runningAway);
            writer.Write(hasNoArrow);
            writer.Write(paperFrames);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            dashing = reader.ReadBoolean();
            throwingArrow = reader.ReadBoolean();
            runningAway = reader.ReadBoolean();
            hasNoArrow = reader.ReadBoolean();
            paperFrames = reader.ReadBoolean();
        }

        public override void FindFrame(int frameHeight)
        {
            frameHeight = 38;
            NPC.spriteDirection = NPC.direction;
            NPC.frameCounter++;
            if (!hasNoArrow && !dashFrames && !throwFrames && !paperFrames)
            {
                if (NPC.frameCounter >= 12)
                {
                    frame += 1;
                    NPC.frameCounter = 0;
                }
                if (frame >= 4)
                {
                    frame = 2;
                }
                if (frame >= 1)
                {
                    frame = 2;
                }
            }
            if (dashFrames)
            {
                if (NPC.frameCounter >= 12 && frame > 5)
                {
                    frame += 1;
                    NPC.frameCounter = 0;
                }
                if (frame >= 6)
                {
                    frame = 4;
                }
                if (frame <= 3)
                {
                    frame = 4;
                }
            }
            if (throwFrames)
            {
                if (NPC.frameCounter >= 20)
                {
                    if (frame != 9)
                    {
                        frame += 1;
                        NPC.frameCounter = 0;
                    }
                    if (frame == 9)
                    {
                        NPC.frameCounter = 0;
                        frame = 9;
                        throwFrames = false;
                    }
                }
                if (frame <= 5)
                {
                    frame = 6;
                }
                if (frame >= 10)
                {
                    frame = 9;
                    throwFrames = false;
                }
            }
            if (hasNoArrow)
            {
                if (NPC.frameCounter >= 12)
                {
                    frame += 1;
                    NPC.frameCounter = 0;
                }
                if (frame >= 2)
                {
                    frame = 0;
                }
            }
            if (paperFrames)
            {
                if (NPC.frameCounter >= 12)
                {
                    frame += 1;
                    NPC.frameCounter = 0;
                }
                if (frame <= 9)
                {
                    frame = 10;
                }
                if (frame >= 12)
                {
                    frame = 10;
                }
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StandArrow>(), 1));
        }
    }
}