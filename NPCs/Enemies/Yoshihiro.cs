using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.NPCs.Enemies
{
    public class Yoshihiro : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 12; //this defines how many frames the npc sprite sheet has
        }

        public override void SetDefaults()
        {
            npc.width = 38; //the npc sprite width
            npc.height = 32;  //the npc sprite height
            npc.defense = 13;  //the npc defense
            npc.lifeMax = 180;  // the npc life
            npc.HitSound = SoundID.NPCHit1;  //the npc sound when is hit
            npc.DeathSound = SoundID.NPCDeath1;  //the npc sound when he dies
            npc.knockBackResist = 2f;  //the npc knockback resistance
            npc.chaseable = true;       //whether or not minions can chase this npc
            npc.damage = 14;       //the damage the npc does
            npc.aiStyle = 0;        //no AI, to run void AI()
            npc.noGravity = true;
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
            Player target = Main.player[npc.target];
            if (npc.life <= npc.lifeMax * 0.1)
            {
                runningAway = true;
                npc.netUpdate = true;
            }
            if (npc.position.X > target.position.X)
            {
                npc.velocity.X = baseSpeed + 0.2f;
            }
            if (npc.position.X <= target.position.X)
            {
                npc.velocity.X = -baseSpeed;
            }
            if (runningAway)
            {
                if (npc.position.X > target.position.X)
                {
                    npc.velocity.X = baseSpeed + 0.2f;
                }
                if (npc.position.X <= target.position.X)
                {
                    npc.velocity.X = -baseSpeed;
                }
            }
            if (!runningAway && !hasNoArrow && !dashing)
            {
                if (npc.Center.X < target.position.X - 3f)
                {
                    npc.velocity.X = baseSpeed;
                }
                if (npc.Center.X > target.position.X + 3f)
                {
                    npc.velocity.X = -baseSpeed;
                }
                if (npc.Center.X > target.position.X - 3f && npc.Center.X < target.position.X + 3f)
                {
                    npc.velocity.X = 0f;
                }
                if (npc.Center.Y > target.position.Y + 3f)
                {
                    npc.velocity.Y = -baseSpeed;
                }
                if (npc.Center.Y < target.position.Y - 3f)
                {
                    npc.velocity.Y = baseSpeed;
                }
                if (npc.Center.Y < target.position.Y + 3f && npc.Center.Y > target.position.Y - 3f)       //so that he keeps going until he reaches target.position.Y +- 5f
                {
                    npc.velocity.Y = 0f;
                }
                npc.netUpdate = true;
            }
            if (!dashing)
            {
                if (npc.velocity.X > 0)
                {
                    npc.direction = 1;
                    tileDetectionAddition = 3;
                }
                if (npc.velocity.X < 0)
                {
                    npc.direction = -1;
                    tileDetectionAddition = -1;
                }
            }
            npc.spriteDirection = npc.direction;
            if (npc.ai[0] > 0f)     //an attack timer
            {
                npc.ai[0] -= 1f;
            }
            if (npc.ai[1] > 0f)     //dash timer
            {
                npc.velocity.X = 15 * npc.direction;
                npc.ai[1] -= 1f;
            }
            if (WorldGen.SolidTile((int)(npc.position.X / 16) + tileDetectionAddition, (int)(npc.position.Y / 16f)) || WorldGen.SolidTile((int)(npc.position.X / 16) + tileDetectionAddition, (int)(npc.position.Y / 16f) + 1) || WorldGen.SolidTile((int)(npc.position.X / 16) + tileDetectionAddition, (int)(npc.position.Y / 16f) + 2))
            {
                npc.ai[3] += 1f;
            }
            if (npc.ai[3] >= 180f)
            {
                paperFrames = true;
                npc.noTileCollide = true;
            }
            if (paperFrames)
            {
                npc.ai[3] -= 1f;
                npc.noTileCollide = true;
                if (npc.ai[3] <= 0)
                {
                    npc.noTileCollide = false;
                    paperFrames = false;
                }
                npc.netUpdate = true;
            }
            if (npc.ai[0] <= 0f && !hasNoArrow && !dashing && !throwingArrow && !paperFrames && !runningAway)
            {
                targetDistance = Vector2.Distance(npc.Center, target.Center);
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
                    npc.ai[0] += 30f;
                    Vector2 shootVel = target.Center - npc.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= 16f;
                    yosarrowWhoAmI = Projectile.NewProjectile(Projectile.GetSource_FromThis(), npc.Center.X, npc.Center.Y, shootVel.X, shootVel.Y, ModContent.ProjectileType<Yosarrow>()), 21, 1f);
                    Main.projectile[yosarrowWhoAmI].netUpdate = true;
                    npc.netUpdate = true;
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
                    targetDistance = Vector2.Distance(npc.Center, targetProjectile.Center);
                    if (npc.Center.X < targetProjectile.position.X - 5f)
                    {
                        npc.velocity.X = 5f;
                        npc.direction = 1;
                    }
                    if (npc.Center.X > targetProjectile.position.X + 5f)
                    {
                        npc.velocity.X = -5f;
                        npc.direction = -1;
                    }
                    if (npc.Center.X > targetProjectile.position.X - 5f && npc.Center.X < targetProjectile.position.X + 5f)
                    {
                        npc.velocity.X = 0f;
                    }
                    if (npc.Center.Y > targetProjectile.position.Y + 5f)
                    {
                        npc.velocity.Y = -5f;
                    }
                    if (npc.Center.Y < targetProjectile.position.Y - 5f)
                    {
                        npc.velocity.Y = 5f;
                    }
                    if (npc.Center.Y < targetProjectile.position.Y + 5f && npc.Center.Y > targetProjectile.position.Y - 5f)
                    {
                        npc.velocity.Y = 0f;
                    }
                    if (targetDistance < 20f)
                    {
                        Main.projectile[yosarrowWhoAmI].Kill();
                        yosarrowWhoAmI = -1;
                        npc.ai[0] += 150f + Main.rand.Next(0, 31);
                        npc.netUpdate = true;
                        hasNoArrow = false;
                    }
                }
                if (yosarrowWhoAmI == -1)       //in case the projectlie dies somehow
                {
                    npc.ai[0] += 150f + Main.rand.Next(0, 31);
                    hasNoArrow = false;
                    npc.netUpdate = true;
                }
            }
            if (dashing)
            {
                dashFrames = true;
                if (npc.ai[1] <= 0f)
                {
                    npc.ai[1] += 45f;
                    if (npc.Center.X < target.Center.X)
                    {
                        timesDashed += 1;
                        npc.ai[2] = 1f;
                    }
                    if (npc.Center.X > target.Center.X)
                    {
                        timesDashed += 1;
                        npc.ai[2] = -1f;
                    }
                }
                if (npc.ai[2] == 1f)
                {
                    npc.velocity.X = 15f;
                    npc.velocity.Y = 0f;
                    npc.direction = 1;
                }
                if (npc.ai[2] == -1f)
                {
                    npc.velocity.X = -15f;
                    npc.velocity.Y = 0f;
                    npc.direction = -1;
                }
                if (npc.ai[2] != 0 && npc.ai[1] <= 10f)
                {
                    npc.ai[2] = 0f;
                    npc.velocity = Vector2.Zero;
                }
                if (timesDashed >= 3)
                {
                    timesDashed = 0;
                    npc.ai[0] += 150f + Main.rand.Next(0, 31);
                    dashFrames = false;
                    dashing = false;
                }
                npc.netUpdate = true;
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
            npc.spriteDirection = npc.direction;
            npc.frameCounter++;
            if (!hasNoArrow && !dashFrames && !throwFrames && !paperFrames)
            {
                if (npc.frameCounter >= 12)
                {
                    frame += 1;
                    npc.frameCounter = 0;
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
                if (npc.frameCounter >= 12 && frame > 5)
                {
                    frame += 1;
                    npc.frameCounter = 0;
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
                if (npc.frameCounter >= 20)
                {
                    if (frame != 9)
                    {
                        frame += 1;
                        npc.frameCounter = 0;
                    }
                    if (frame == 9)
                    {
                        npc.frameCounter = 0;
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
                if (npc.frameCounter >= 12)
                {
                    frame += 1;
                    npc.frameCounter = 0;
                }
                if (frame >= 2)
                {
                    frame = 0;
                }
            }
            if (paperFrames)
            {
                if (npc.frameCounter >= 12)
                {
                    frame += 1;
                    npc.frameCounter = 0;
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
            npc.frame.Y = frame * frameHeight;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ModContent.ItemType<StandArrow>()), 1);
        }
    }
}