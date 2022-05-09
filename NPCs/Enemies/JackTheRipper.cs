using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.NPCs.Enemies
{
    public class JackTheRipper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 19;
        }

        public override void SetDefaults()
        {
            npc.width = 48;
            npc.height = 54;
            npc.defense = 24;
            npc.lifeMax = 300;
            npc.damage = 40;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 1f;
            npc.chaseable = true;
            npc.noGravity = false;
            npc.daybreak = true;
            npc.aiStyle = 3;
            aiType = 73;
        }

        public const int IdleFrame = 0;
        public const int JumpingFrame = 1;
        public const int WalkingFramesMinimum = 2;
        public const int WalkingFramesMaximum = 14;
        public const int KnivesFramesMinimum = 15;
        public const int KnivesFramesMaximum = 17;
        public const int GrabFrame = 18;

        private int frame = 0;
        private int runCounter = 200;
        private int dashCounter = 0;
        private int dashCooldown = 0;
        private int knifeThrowTimer = 0;
        private int knivesCooldown = 200;
        private int npcVictimWhoAmI = -1;
        private int expertboost = 1;

        private bool dashing = false;
        private bool throwingKnives = false;
        private bool alreadyHidOnce = false;
        private bool grabbingPlayer = false;
        private bool currentlyHidden = false;

        public override void AI()
        {
            Player target = Main.player[npc.target];
            if (target.dead || npc.target == -1)
            {
                npc.TargetClosest();
            }
            if (knivesCooldown > 0)
            {
                knivesCooldown -= 1;
            }
            if (dashCooldown > 0)
            {
                dashCooldown -= 1;
            }
            if (npc.life > npc.lifeMax)
            {
                npc.life = npc.lifeMax;
            }

            if (!currentlyHidden)
            {
                npc.hide = false;
                npc.immortal = false;
                npc.AddBuff(ModContent.BuffType<Vampire>(), 2);
                if (!grabbingPlayer)
                {
                    if (npc.HasBuff(ModContent.BuffType<Sunburn>()))
                    {
                        npc.defense = 0;
                        npc.damage = 25 * expertboost;
                    }
                    else
                    {
                        npc.defense = 24;
                        npc.damage = 40 * expertboost;
                    }
                }
                if (grabbingPlayer)
                {
                    if (npc.HasBuff(ModContent.BuffType<Sunburn>()))
                    {
                        npc.defense = 0;
                        npc.damage = 0;
                    }
                    else
                    {
                        npc.defense = 24;
                        npc.damage = 0;
                    }
                }
                if (npc.life <= npc.lifeMax / 2)
                {
                    runCounter--;
                    if (!throwingKnives && !dashing)
                    {
                        if (runCounter > 0)
                        {
                            if (npc.position.X >= target.position.X)
                            {
                                npc.direction = 1;
                                npc.velocity.X = 2f;
                            }
                            if (npc.position.X < target.position.X)
                            {
                                npc.direction = -1;
                                npc.velocity.X = -2f;
                            }
                        }
                        else
                        {
                            if (npc.position.X - 200 >= target.position.X)
                            {
                                npc.direction = 1;
                                npc.velocity.X = 2f;
                            }
                            if (npc.position.X + 200 < target.position.X)
                            {
                                npc.direction = -1;
                                npc.velocity.X = -2f;
                            }
                        }

                    }
                    if (!target.dead && !npc.noTileCollide && npc.Distance(target.Center) <= 200f && dashCooldown <= 0 && !throwingKnives && runCounter <= 0)
                    {
                        dashing = true;
                    }
                    if (dashing && !grabbingPlayer)
                    {
                        aiType = 199;
                        npc.aiStyle = 3;
                        npc.knockBackResist = 0f;

                        dashCounter += 1;
                        if (npc.position.X >= target.position.X)
                        {
                            npc.direction = -1;
                            npc.velocity.X = -4f;
                        }
                        if (npc.position.X < target.position.X)
                        {
                            npc.direction = 1;
                            npc.velocity.X = 4f;
                        }
                        if (dashCounter >= 200)
                        {
                            dashCounter = 0;
                            dashCooldown = 800;
                            dashing = false;
                        }
                    }
                    if (grabbingPlayer)
                    {
                        npc.damage = 0;
                        npc.aiStyle = 0;
                        npc.knockBackResist = 0f;

                        if (target.mount.Type != 0)
                        {
                            target.mount.Dismount(target);
                        }
                        target.position = npc.position;     //I assume it's a sort of stunlock so I'll leave it as is
                        target.AddBuff(BuffID.Suffocation, 2);   
                        target.npcTypeNoAggro[Mod.NPCType("JackTheRipper>()] = true;
                        if (target.dead)
                        {
                            dashCounter = 0;
                            dashCooldown = 800;
                            grabbingPlayer = false;
                            dashing = false;
                        }
                    }
                    Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Blood, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f);
                }
                if (!npc.noTileCollide && npc.Distance(target.Center) <= 400f && knivesCooldown <= 0 && !dashing && !target.dead)
                {
                    throwingKnives = true;
                }
                if (throwingKnives)
                {
                    frame = 3;
                    npc.aiStyle = 0;
                    npc.velocity.X = 0;
                    knifeThrowTimer += 1;

                    Vector2 shootVel = target.Center - npc.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 0f);
                    }
                    shootVel.Normalize();

                    if (knifeThrowTimer == 30)
                    {
                        ThrowKnives(shootVel, 2);
                    }
                    if (knifeThrowTimer == 40)
                    {
                        ThrowKnives(shootVel, 1);
                    }
                    if (knifeThrowTimer == 50)
                    {
                        ThrowKnives(shootVel, 2);
                    }
                    if (knifeThrowTimer == 60)
                    {
                        ThrowKnives(shootVel, 1);
                    }
                    if (knifeThrowTimer >= 90)
                    {
                        knifeThrowTimer = 0;
                        knivesCooldown = 600;
                        throwingKnives = false;
                    }

                    if (npc.position.X >= target.position.X)
                    {
                        npc.direction = 1;
                    }
                    if (npc.position.X < target.position.X)
                    {
                        npc.direction = -1;
                    }
                }
                if (npc.velocity.X > 0)
                {
                    npc.spriteDirection = 1;
                }
                if (npc.velocity.X < 0)
                {
                    npc.spriteDirection = -1;
                }
                if (!throwingKnives && !dashing)
                {
                    aiType = 73;
                    npc.aiStyle = 3;
                    npc.knockBackResist = 0.5f;
                }
            }
            else       //Searches for Town NPCs to hide in
            {
                /*for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC otherNPCs = Main.npc[n];
                    JoJoGlobalNPC jojoNPC = otherNPCs.GetGlobalNPC<JoJoGlobalNPC>());
                    if (otherNPCs.townNPC)
                    {
                        if (jojoNPC.jackHiding)
                        {
                            if (npc.whoAmI == jojoNPC.hiddenJackWhoAmI)
                            {
                                if (otherNPCs.active)
                                {
                                    npc.life = npc.lifeMax;
                                    npc.Center = otherNPCs.Center;
                                    npc.aiStyle = 0;
                                    npc.damage = 0;
                                    npc.immortal = true;
                                    npc.hide = true;
                                    if (otherNPCs.Distance(target.Center) <= 200f && npc.whoAmI == jojoNPC.hiddenJackWhoAmI)
                                    {
                                        otherNPCs.AddBuff(ModContent.BuffType<Dead>(), 2);
                                    }
                                }
                                if (!otherNPCs.active)
                                {
                                    if (npc.whoAmI == jojoNPC.hiddenJackWhoAmI)
                                    {
                                        currentlyHidden = false;
                                        alreadyHidOnce = true;
                                        jojoNPC.hiddenJackWhoAmI = 0;
                                        npc.whoAmI = 0;
                                        jojoNPC.jackHiding = false;
                                    }
                                }
                            }
                        }
                    }
                }*/
                if (npcVictimWhoAmI == -1)      //Don't run the code of the captured NPC doesn't exist
                    return;

                NPC victimNPC = Main.npc[npcVictimWhoAmI];      //Rather than scanning for NPCs for which one is being grabbed we can instead just save the NPC it's holding and directly use it
                if (victimNPC.active)
                {
                    npc.damage = 0;
                    npc.aiStyle = 0;
                    npc.life = npc.lifeMax;
                    npc.hide = true;
                    npc.immortal = true;

                    npc.Center = victimNPC.Center;
                    if (victimNPC.Distance(target.Center) <= 200f)
                    {
                        victimNPC.AddBuff(ModContent.BuffType<Dead>(), 2);
                    }
                }
                else
                {
                    npcVictimWhoAmI = -1;
                    alreadyHidOnce = true;
                    currentlyHidden = false;
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (npc.life < npc.lifeMax && !currentlyHidden)
            {
                int lifeStealAmount = damage / 2;
                npc.life += lifeStealAmount;
            }
            if (dashing)
            {
                grabbingPlayer = true;
                npc.damage = 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[npc.target];
            if (target.townNPC && npc.life > npc.lifeMax / 2 && !currentlyHidden && !alreadyHidOnce && npc.Distance(player.Center) >= 400f)
            {
                currentlyHidden = true;
                npcVictimWhoAmI = target.whoAmI;
            }
            if (npc.life < npc.lifeMax && !currentlyHidden)
            {
                int lifeStealAmount = damage / 2;
                npc.life += lifeStealAmount;
            }
        }

        private void ThrowKnives(Vector2 normalizedShootVelocity, int style)
        {
            if (style == 1)
            {
                normalizedShootVelocity *= 55f;
                knifeThrowTimer += 1;
                float numberKnives = 3;
                float rotationk = MathHelper.ToRadians(3);
                for (int i = 0; i < numberKnives; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(normalizedShootVelocity.X, normalizedShootVelocity.Y).RotatedBy(MathHelper.Lerp(-rotationk, rotationk, i / (numberKnives - 1))) * .2f;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), npc.Center.X, npc.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<JackKnife>(), 6 * expertboost, 2f);
                    Main.projectile[proj].netUpdate = true;
                    npc.netUpdate = true;
                }
            }
            if (style == 2)
            {
                normalizedShootVelocity *= 55f;
                float numberKnives = 2;
                float rotationk = MathHelper.ToRadians(3);
                for (int i = 0; i < numberKnives; i++)
                {
                    Vector2 perturbedSpeed = new Vector2(normalizedShootVelocity.X, normalizedShootVelocity.Y).RotatedBy(MathHelper.Lerp(-rotationk, rotationk, i / (numberKnives - 1))) * .2f;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), npc.Center.X, npc.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<JackKnife>(), 6 * expertboost, 2f);
                    Main.projectile[proj].netUpdate = true;
                    npc.netUpdate = true;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (dashing && !grabbingPlayer)
            {
                SpriteEffects effects = (npc.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Texture2D texture2d = Mod.GetTexture("NPCs/Enemies/JackTheRipper>();
                Vector2 vector2 = new Vector2((Main.npcTexture[npc.type].Width / 2), (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
                Main.spriteBatch.Draw(texture2d, new Vector2(npc.position.X - Main.screenPosition.X + (npc.width / 2) - Main.npcTexture[npc.type].Width * npc.scale / 2f + vector2.X * npc.scale, npc.position.Y - Main.screenPosition.Y + npc.height - Main.npcTexture[npc.type].Height * npc.scale / Main.npcFrameCount[npc.type] + 4f + vector2.Y * npc.scale), new Rectangle?(npc.frame), Color.White, npc.rotation, vector2, npc.scale, effects, 0f);
                for (int i = 1; i < npc.oldPos.Length; i++)
                {
                    Color color = Lighting.GetColor((int)(npc.position.X + npc.width * 0.5) / 16, (int)((npc.position.Y + npc.height * 0.5) / 16.0));
                    Color color2 = color;
                    color2 = Color.Lerp(color2, Color.Transparent, 0.5f);
                    color2 = npc.GetAlpha(color2);
                    color2 *= (npc.oldPos.Length - i) / 15f;
                    Main.spriteBatch.Draw(texture2d, new Vector2(npc.position.X - Main.screenPosition.X + (npc.width / 2) - Main.npcTexture[npc.type].Width * npc.scale / 2f + vector2.X * npc.scale, npc.position.Y - Main.screenPosition.Y + npc.height - Main.npcTexture[npc.type].Height * npc.scale / Main.npcFrameCount[npc.type] + 4f + vector2.Y * npc.scale) - npc.velocity * i * 0.5f, new Rectangle?(npc.frame), color2, npc.rotation, vector2, npc.scale, effects, 0f);
                }
            }
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            frameHeight = npc.height;
            if (npc.velocity.X == 0f)
            {
                frame = IdleFrame;
                if (grabbingPlayer)
                {
                    frame = GrabFrame;
                }
                npc.frameCounter = 0;
            }
            else
            {
                npc.frameCounter += Math.Abs(npc.velocity.X);
                if (npc.frameCounter >= 6)
                {
                    frame++;
                    npc.frameCounter = 0;
                    if (frame > WalkingFramesMaximum)
                    {
                        frame = WalkingFramesMinimum;
                    }
                    if (frame < WalkingFramesMinimum)
                    {
                        frame = WalkingFramesMinimum;
                    }
                }
            }
            if (knifeThrowTimer != 0)
            {
                npc.frameCounter++;
                if (knifeThrowTimer <= 30)
                {
                    if (npc.frameCounter % 10 == 0)
                    {
                        frame++;
                        npc.frameCounter = 0;
                    }
                }
                if (knifeThrowTimer > 30 && knifeThrowTimer <= 60)
                {
                    if (frame != KnivesFramesMaximum)
                    {
                        if (npc.frameCounter >= 3)
                        {
                            frame++;
                            npc.frameCounter = 0;
                        }
                    }
                    else
                    {
                        if (npc.frameCounter >= 4)
                        {
                            frame++;
                            npc.frameCounter = 0;
                        }
                    }
                }

                if (frame < KnivesFramesMinimum)
                {
                    frame = KnivesFramesMinimum;
                }
                if (frame > KnivesFramesMaximum)
                {
                    frame = KnivesFramesMinimum;
                }
            }
            if (npc.velocity.Y != 0f)
            {
                frame = JumpingFrame;
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float chance = 0f;
            if (JoJoStandsWorld.VampiricNight)
            {
                chance = 0.01f;
            }
            return chance;
        }
    }
}