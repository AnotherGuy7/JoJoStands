using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class WangChan : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 40;
            npc.height = 48;
            npc.defense = 24;
            npc.lifeMax = 150;
            npc.damage = 60; 
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 1f;
            npc.chaseable = true;
            npc.noGravity = false;
            npc.daybreak = true;
            npc.aiStyle = 3;
            aiType = 199;
        }

        private int frame = 0;
        private int runCounter = 0;
        private int poisonThrowTimer = 0;
        private int poisonCooldown = 0;
        private int expertboost = 1;
        private float JumpCooldown = 0f;
        private float radians = 0f;
        private bool poisonThrow = false;

        public override void AI()
        {
            Player target = Main.player[npc.target];
            if (runCounter > 0)
            {
                runCounter -= 1;
            }
            if (target.dead || npc.target == -1)
            {
                npc.TargetClosest();
            }
            npc.AddBuff(mod.BuffType("Vampire"), 2);
            if (npc.HasBuff(mod.BuffType("Sunburn")))
            {
                npc.defense = 0;
                npc.damage = 20 * expertboost;
            }
            else
            {
                npc.defense = 24;
                npc.damage = 60 * expertboost;
            }

            if (Main.expertMode)
            {
                expertboost = 2;
            }

            if (npc.velocity.X > 0)
            {
                npc.spriteDirection = 1;
            }
            if (npc.velocity.X < 0)
            {
                npc.spriteDirection = -1;
            }

            if (poisonCooldown > 0)
            {
                poisonCooldown -= 1;
            }

            if (npc.life > npc.lifeMax)
            {
                npc.life = npc.lifeMax;
            }

            if (!poisonThrow)
            {
                aiType = 199;
                npc.aiStyle = 3;
                if (npc.collideY && JumpCooldown == 0f && npc.velocity.Y == 0) //jump jump jump
                {
                    JumpCooldown = 45f;
                    npc.velocity.Y -= 9f;
                }
                if (npc.velocity.Y < 3f)
                {
                    npc.velocity.Y += 0.15f;
                }
                if (JumpCooldown > 0f)
                {
                    JumpCooldown--;
                }
            }

            if (runCounter == 0)
            {
                if (npc.position.X >= target.position.X)
                {

                    if (npc.position.X - 50 >= target.position.X)
                    {
                        npc.direction = -1;
                        npc.velocity.X = -3f;
                    }
                }
                if (npc.position.X < target.position.X)
                {
                    if (npc.position.X + 50 < target.position.X)
                    {
                        npc.direction = 1;
                        npc.velocity.X = 3f;
                    }
                }
            }
            if (runCounter != 0)
            {
                if (npc.position.X >= target.position.X)
                {
                    npc.direction = 1;
                    npc.velocity.X = 3f;
                }
                if (npc.position.X < target.position.X)
                {
                    npc.direction = -1;
                    npc.velocity.X = -3f;
                }
            }

            if (npc.Distance(target.Center) >= 300f && runCounter > 0)
            {
                runCounter = 0;
                poisonCooldown = 0;
            }

            if (!npc.noTileCollide && npc.Distance(target.Center) <= 300f && poisonCooldown <= 0 && !target.dead) //poison
            {
                poisonThrow = true;
            }
            if (poisonThrow)
            {
                frame = 3;
                npc.aiStyle = 0;
                npc.velocity.X = 0;
                poisonThrowTimer += 1;
                if (poisonThrowTimer == 45)
                {
                    Vector2 shootVel = target.Center - npc.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 0f);
                    }
                    shootVel.Normalize();
                    shootVel *= 10f;
                    if (target.position.X < npc.position.X)
                    {
                        radians = 30f;
                    }
                    if (target.position.X > npc.position.X)
                    {
                        radians = -30f;
                    }
                    float rotation = MathHelper.ToRadians(radians);
                    Vector2 NewSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, 1) * 1f);
                    int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, NewSpeed.X, NewSpeed.Y, mod.ProjectileType("AcidVenomFlask"), 21 * expertboost, 1f);
                    Main.projectile[proj].netUpdate = true;
                    npc.netUpdate = true;
                }
                if (poisonThrowTimer >= 90)
                {
                    poisonThrowTimer = 0;
                    poisonCooldown = 800;
                    runCounter = 0;
                    poisonThrow = false;
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (npc.life < npc.lifeMax)
            {
                int lifeStealAmount = damage / 2;
                npc.life += lifeStealAmount;
            }
            if (!poisonThrow && runCounter == 0)
            {
                runCounter += 200;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (npc.life < npc.lifeMax)
            {
                int lifeStealAmount = damage / 2;
                npc.life += lifeStealAmount;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            frameHeight = 48;
            if (npc.velocity != Vector2.Zero)
            {
                npc.frameCounter += Math.Abs(npc.velocity.X);
                if (npc.frameCounter >= 10)
                {
                    frame += 1;
                    npc.frameCounter = 0;
                }
                if (frame >= 3)
                {
                    frame = 0;
                }
            }
            npc.frame.Y = frame * frameHeight;
        }
    }
}