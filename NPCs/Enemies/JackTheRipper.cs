using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Buffs.Debuffs;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs.Enemies
{
    public class JackTheRipper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 19;
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 54;
            NPC.defense = 24;
            NPC.lifeMax = 300;
            NPC.damage = 40;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 1f;
            NPC.chaseable = true;
            NPC.noGravity = false;
            NPC.daybreak = true;
            NPC.aiStyle = 3;
            AIType = 73;
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
        private int NPCVictimWhoAmI = -1;
        private int expertboost = 1;

        private bool dashing = false;
        private bool throwingKnives = false;
        private bool alreadyHidOnce = false;
        private bool grabbingPlayer = false;
        private bool currentlyHidden = false;

        public override void AI()
        {
            Player target = Main.player[NPC.target];
            if (target.dead || NPC.target == -1)
            {
                NPC.TargetClosest();
            }
            if (knivesCooldown > 0)
            {
                knivesCooldown -= 1;
            }
            if (dashCooldown > 0)
            {
                dashCooldown -= 1;
            }
            if (NPC.life > NPC.lifeMax)
            {
                NPC.life = NPC.lifeMax;
            }

            if (!currentlyHidden)
            {
                NPC.hide = false;
                NPC.immortal = false;
                NPC.AddBuff(ModContent.BuffType<Vampire>(), 2);
                if (!grabbingPlayer)
                {
                    if (NPC.HasBuff(ModContent.BuffType<Sunburn>()))
                    {
                        NPC.defense = 0;
                        NPC.damage = 25 * expertboost;
                    }
                    else
                    {
                        NPC.defense = 24;
                        NPC.damage = 40 * expertboost;
                    }
                }
                if (grabbingPlayer)
                {
                    if (NPC.HasBuff(ModContent.BuffType<Sunburn>()))
                    {
                        NPC.defense = 0;
                        NPC.damage = 0;
                    }
                    else
                    {
                        NPC.defense = 24;
                        NPC.damage = 0;
                    }
                }
                if (NPC.life <= NPC.lifeMax / 2)
                {
                    runCounter--;
                    if (!throwingKnives && !dashing)
                    {
                        if (runCounter > 0)
                        {
                            if (NPC.position.X >= target.position.X)
                            {
                                NPC.direction = 1;
                                NPC.velocity.X = 2f;
                            }
                            if (NPC.position.X < target.position.X)
                            {
                                NPC.direction = -1;
                                NPC.velocity.X = -2f;
                            }
                        }
                        else
                        {
                            if (NPC.position.X - 200 >= target.position.X)
                            {
                                NPC.direction = 1;
                                NPC.velocity.X = 2f;
                            }
                            if (NPC.position.X + 200 < target.position.X)
                            {
                                NPC.direction = -1;
                                NPC.velocity.X = -2f;
                            }
                        }

                    }
                    if (!target.dead && !NPC.noTileCollide && NPC.Distance(target.Center) <= 200f && dashCooldown <= 0 && !throwingKnives && runCounter <= 0)
                    {
                        dashing = true;
                    }
                    if (dashing && !grabbingPlayer)
                    {
                        AIType = 199;
                        NPC.aiStyle = 3;
                        NPC.knockBackResist = 0f;

                        dashCounter += 1;
                        if (NPC.position.X >= target.position.X)
                        {
                            NPC.direction = -1;
                            NPC.velocity.X = -4f;
                        }
                        if (NPC.position.X < target.position.X)
                        {
                            NPC.direction = 1;
                            NPC.velocity.X = 4f;
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
                        NPC.damage = 0;
                        NPC.aiStyle = 0;
                        NPC.knockBackResist = 0f;

                        if (target.mount.Type != 0)
                        {
                            target.mount.Dismount(target);
                        }
                        target.position = NPC.position;     //I assume it's a sort of stunlock so I'll leave it as is
                        target.AddBuff(BuffID.Suffocation, 2);
                        target.npcTypeNoAggro[ModContent.NPCType<JackTheRipper>()] = true;
                        if (target.dead)
                        {
                            dashCounter = 0;
                            dashCooldown = 800;
                            grabbingPlayer = false;
                            dashing = false;
                        }
                    }
                    Dust.NewDust(NPC.position + NPC.velocity, NPC.width, NPC.height, DustID.Blood, NPC.velocity.X * -0.5f, NPC.velocity.Y * -0.5f);
                }
                if (!NPC.noTileCollide && NPC.Distance(target.Center) <= 400f && knivesCooldown <= 0 && !dashing && !target.dead)
                {
                    throwingKnives = true;
                }
                if (throwingKnives)
                {
                    frame = 3;
                    NPC.aiStyle = 0;
                    NPC.velocity.X = 0;
                    knifeThrowTimer += 1;

                    Vector2 shootVel = target.Center - NPC.Center;
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

                    if (NPC.position.X >= target.position.X)
                    {
                        NPC.direction = 1;
                    }
                    if (NPC.position.X < target.position.X)
                    {
                        NPC.direction = -1;
                    }
                }
                if (NPC.velocity.X > 0)
                {
                    NPC.spriteDirection = 1;
                }
                if (NPC.velocity.X < 0)
                {
                    NPC.spriteDirection = -1;
                }
                if (!throwingKnives && !dashing)
                {
                    AIType = 73;
                    NPC.aiStyle = 3;
                    NPC.knockBackResist = 0.5f;
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
                            if (NPC.whoAmI == jojoNPC.hiddenJackWhoAmI)
                            {
                                if (otherNPCs.active)
                                {
                                    NPC.life = NPC.lifeMax;
                                    NPC.Center = otherNPCs.Center;
                                    NPC.aiStyle = 0;
                                    NPC.damage = 0;
                                    NPC.immortal = true;
                                    NPC.hide = true;
                                    if (otherNPCs.Distance(target.Center) <= 200f && NPC.whoAmI == jojoNPC.hiddenJackWhoAmI)
                                    {
                                        otherNPCs.AddBuff(ModContent.BuffType<Dead>(), 2);
                                    }
                                }
                                if (!otherNPCs.active)
                                {
                                    if (NPC.whoAmI == jojoNPC.hiddenJackWhoAmI)
                                    {
                                        currentlyHidden = false;
                                        alreadyHidOnce = true;
                                        jojoNPC.hiddenJackWhoAmI = 0;
                                        NPC.whoAmI = 0;
                                        jojoNPC.jackHiding = false;
                                    }
                                }
                            }
                        }
                    }
                }*/
                if (NPCVictimWhoAmI == -1)      //Don't run the code of the captured NPC doesn't exist
                    return;

                NPC victimNPC = Main.npc[NPCVictimWhoAmI];      //Rather than scanning for NPCs for which one is being grabbed we can instead just save the NPC it's holding and directly use it
                if (victimNPC.active)
                {
                    NPC.damage = 0;
                    NPC.aiStyle = 0;
                    NPC.life = NPC.lifeMax;
                    NPC.hide = true;
                    NPC.immortal = true;

                    NPC.Center = victimNPC.Center;
                    if (victimNPC.Distance(target.Center) <= 200f)
                    {
                        victimNPC.AddBuff(ModContent.BuffType<Dead>(), 2);
                    }
                }
                else
                {
                    NPCVictimWhoAmI = -1;
                    alreadyHidOnce = true;
                    currentlyHidden = false;
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (NPC.life < NPC.lifeMax && !currentlyHidden)
            {
                int lifeStealAmount = damage / 2;
                NPC.life += lifeStealAmount;
            }
            if (dashing)
            {
                grabbingPlayer = true;
                NPC.damage = 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[NPC.target];
            if (target.townNPC && NPC.life > NPC.lifeMax / 2 && !currentlyHidden && !alreadyHidOnce && NPC.Distance(player.Center) >= 400f)
            {
                currentlyHidden = true;
                NPCVictimWhoAmI = target.whoAmI;
            }
            if (NPC.life < NPC.lifeMax && !currentlyHidden)
            {
                int lifeStealAmount = damage / 2;
                NPC.life += lifeStealAmount;
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
                    int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<JackKnife>(), 6 * expertboost, 2f);
                    Main.projectile[proj].netUpdate = true;
                    NPC.netUpdate = true;
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
                    int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<JackKnife>(), 6 * expertboost, 2f);
                    Main.projectile[proj].netUpdate = true;
                    NPC.netUpdate = true;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (dashing && !grabbingPlayer)
            {
                SpriteEffects effects = (NPC.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Texture2D texture2d = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/NPCs/Enemies/JackTheRipper").Value;
                Vector2 center = new Vector2((Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width / 2), (Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
                spriteBatch.Draw(texture2d, new Vector2(NPC.position.X - Main.screenPosition.X + (NPC.width / 2) - Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * NPC.scale / 2f + center.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + center.Y * NPC.scale), new Rectangle?(NPC.frame), Color.White, NPC.rotation, center, NPC.scale, effects, 0);
                for (int i = 1; i < NPC.oldPos.Length; i++)
                {
                    Color color = Lighting.GetColor((int)(NPC.position.X + NPC.width * 0.5) / 16, (int)((NPC.position.Y + NPC.height * 0.5) / 16.0));
                    Color color2 = color;
                    color2 = Color.Lerp(color2, Color.Transparent, 0.5f);
                    color2 = NPC.GetAlpha(color2);
                    color2 *= (NPC.oldPos.Length - i) / 15f;
                    spriteBatch.Draw(texture2d, new Vector2(NPC.position.X - Main.screenPosition.X + (NPC.width / 2) - Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * NPC.scale / 2f + center.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + center.Y * NPC.scale) - NPC.velocity * i * 0.5f, new Rectangle?(NPC.frame), color2, NPC.rotation, center, NPC.scale, effects, 0);
                }
            }
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            frameHeight = NPC.height;
            if (NPC.velocity.X == 0f)
            {
                frame = IdleFrame;
                if (grabbingPlayer)
                {
                    frame = GrabFrame;
                }
                NPC.frameCounter = 0;
            }
            else
            {
                NPC.frameCounter += Math.Abs(NPC.velocity.X);
                if (NPC.frameCounter >= 6)
                {
                    frame++;
                    NPC.frameCounter = 0;
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
                NPC.frameCounter++;
                if (knifeThrowTimer <= 30)
                {
                    if (NPC.frameCounter % 10 == 0)
                    {
                        frame++;
                        NPC.frameCounter = 0;
                    }
                }
                if (knifeThrowTimer > 30 && knifeThrowTimer <= 60)
                {
                    if (frame != KnivesFramesMaximum)
                    {
                        if (NPC.frameCounter >= 3)
                        {
                            frame++;
                            NPC.frameCounter = 0;
                        }
                    }
                    else
                    {
                        if (NPC.frameCounter >= 4)
                        {
                            frame++;
                            NPC.frameCounter = 0;
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
            if (NPC.velocity.Y != 0f)
            {
                frame = JumpingFrame;
            }
            NPC.frame.Y = frame * frameHeight;
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