using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.Whitesnake
{
    public class WhitesnakeStandT3 : StandClass
    {
        public override int punchDamage => 69;
        public override int altDamage => 63;
        public override int punchTime => 12;
        public override int halfStandHeight => 44;
        public override float fistWhoAmI => 9f;
        public override int standType => 1;
        public override int standOffset => -20;
        public override float maxDistance => 147f;      //1.5x the normal range cause Whitesnake is considered a long-range stand with melee capabilities
        public override string poseSoundName => "YouWereTwoSecondsTooLate";
        public override string spawnSoundName => "Whitesnake";

        private const float RemoteControlMaxDistance = 50f * 16f;
        private const float SleepingGasEffectRadius = 10f * 16f;

        private int updateTimer = 0;
        private bool stealFrames = false;
        private bool waitingForEnemyFrames = false;
        private bool remoteControlled = false;
        private bool gunRevealFrames = false;
        private bool remoteControlFrames = false;
        private int armFrame = 0;
        private float armRotation = 0;
        private float floatTimer = 0;
        private bool canShootAgain = false;
        private Vector2 armPosition;
        private int sleepingGasTimer = 0;
        private Vector2 sleepingGasFormPosition;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            remoteControlFrames = remoteControlled;
            if (!mPlayer.standAutoMode && !remoteControlled)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !stealFrames)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (Main.mouseRight && shootCount <= 0 && projectile.owner == Main.myPlayer)
                {
                    projectile.frame = 0;
                    secondaryAbilityFrames = true;
                }
                if (secondaryAbilityFrames)
                {
                    Main.mouseLeft = false;
                    if (projectile.frame >= 4)
                    {
                        shootCount += 120;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 8f;
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("MeltYourHeart"), (int)(altDamage * mPlayer.standDamageBoosts), 2f, projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                        secondaryAbilityFrames = false;
                    }
                }
                if (!attackFrames && !stealFrames && !waitingForEnemyFrames && !gunRevealFrames)
                {
                    if (!secondaryAbilityFrames)
                        StayBehind();
                    else
                        GoInFront();
                }
                if (SpecialKeyCurrent() && projectile.owner == Main.myPlayer && shootCount <= 0 && !stealFrames)
                {
                    projectile.velocity = Main.MouseWorld - projectile.position;
                    projectile.velocity.Normalize();
                    projectile.velocity *= 5f;

                    float mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        projectile.velocity = player.velocity + projectile.velocity;
                    }
                    if (mouseDistance <= 40f)
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                    waitingForEnemyFrames = true;
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active)
                        {
                            if (projectile.Distance(npc.Center) <= 30f && !npc.boss && !npc.immortal && !npc.hide)
                            {
                                projectile.ai[0] = npc.whoAmI;
                                stealFrames = true;
                            }
                        }
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        for (int p = 0; p < Main.maxPlayers; p++)
                        {
                            Player otherPlayer = Main.player[p];
                            if (otherPlayer.active)
                            {
                                if (projectile.Distance(otherPlayer.Center) <= 30f & otherPlayer.team != player.team && otherPlayer.whoAmI != player.whoAmI)
                                {
                                    projectile.ai[1] = otherPlayer.whoAmI;
                                    stealFrames = true;
                                }
                            }
                        }
                    }
                    LimitDistance();
                }
                if (stealFrames && projectile.ai[0] != -1f)
                {
                    projectile.velocity = Vector2.Zero;
                    NPC npc = Main.npc[(int)projectile.ai[0]];
                    npc.direction = -projectile.direction;
                    npc.position = projectile.position + new Vector2(-6f * projectile.direction, -2f - npc.height / 3f);
                    npc.velocity = Vector2.Zero;
                    if (projectile.frame == 4)
                    {
                        npc.AddBuff(mod.BuffType("Stolen"), 30 * 60);
                    }
                    if (projectile.frame == 6)
                    {
                        stealFrames = false;
                        projectile.ai[0] = -1f;
                        shootCount += 60;
                    }
                    if (!npc.active)
                    {
                        stealFrames = false;
                        projectile.ai[0] = -1f;
                        shootCount += 30;
                    }
                    LimitDistance();
                }
                if (stealFrames && projectile.ai[1] != -1f)
                {
                    projectile.velocity = Vector2.Zero;
                    Player otherPlayer = Main.player[(int)projectile.ai[1]];
                    otherPlayer.direction = -projectile.direction;
                    otherPlayer.position = projectile.position + new Vector2(-6f * projectile.direction, -2f - otherPlayer.height / 3f);
                    otherPlayer.velocity = Vector2.Zero;
                    if (projectile.frame == 4)      //this is the frame where the disc has just been stolen
                    {
                        otherPlayer.AddBuff(mod.BuffType("Stolen"), 30 * 60);
                    }
                    if (projectile.frame == 6)      //anim ended
                    {
                        stealFrames = false;
                        projectile.ai[1] = -1f;
                        shootCount += 60;
                    }
                    if (!otherPlayer.active)
                    {
                        stealFrames = false;
                        projectile.ai[1] = -1f;
                        shootCount += 30;
                    }
                    LimitDistance();
                }
                if (!SpecialKeyCurrent() && ((stealFrames || waitingForEnemyFrames) || projectile.ai[1] != -1f))
                {
                    stealFrames = false;
                    waitingForEnemyFrames = false;
                    projectile.ai[0] = -1f;
                    projectile.ai[1] = -1f;
                    shootCount += 30;
                }

                if (SecondSpecialKeyPressedNoCooldown() && shootCount <= 0 && !gunRevealFrames && !stealFrames)
                {
                    shootCount += 15;
                    attackFrames = false;
                    normalFrames = false;
                    secondaryAbilityFrames = false;
                    waitingForEnemyFrames = false;
                    projectile.frame = 0;
                    projectile.frameCounter = 0;
                    gunRevealFrames = true;
                }
            }
            if (!mPlayer.standAutoMode && remoteControlled)
            {
                mPlayer.standRemoteMode = true;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);

                floatTimer += 0.06f;
                armRotation = (Main.MouseWorld - projectile.Center).ToRotation();
                armPosition = projectile.Center + new Vector2(-18f * projectile.spriteDirection, -4f);
                if (projectile.spriteDirection == -1)
                {
                    armPosition += new Vector2(6f, -8f);
                }
                HandleDrawOffsets();
                if (shootCount > 0)
                    armFrame = 1;
                else
                    armFrame = 0;

                bool aboveTile = Collision.SolidTiles((int)projectile.Center.X / 16, (int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, (int)(projectile.Center.Y / 16) + 4);
                if (aboveTile)
                {
                    projectile.velocity.Y = (float)Math.Sin(floatTimer) / 5f;
                }
                else
                {
                    if (projectile.velocity.Y < 6f)
                    {
                        projectile.velocity.Y += 0.2f;
                    }
                    if (Vector2.Distance(projectile.Center, player.Center) >= RemoteControlMaxDistance)
                    {
                        projectile.velocity = player.Center - projectile.Center;
                        projectile.velocity.Normalize();
                        projectile.velocity *= 0.8f;
                    }
                }

                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    Vector2 moveVelocity = Main.MouseWorld - projectile.Center;
                    moveVelocity.Normalize();
                    projectile.velocity.X = moveVelocity.X * 4.5f;
                    if (aboveTile)
                        projectile.velocity.Y += moveVelocity.Y * 2f;

                    projectile.direction = 1;
                    if (Main.MouseWorld.X < projectile.Center.X)
                    {
                        projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

                    if (Vector2.Distance(projectile.Center, player.Center) >= RemoteControlMaxDistance)
                    {
                        projectile.velocity = player.Center - projectile.Center;
                        projectile.velocity.Normalize();
                        projectile.velocity *= 0.8f;
                    }
                    projectile.netUpdate = true;
                }
                if (!Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    projectile.velocity.X *= 0.78f;
                    projectile.netUpdate = true;
                }
                if (Main.mouseRight && canShootAgain && shootCount <= 0 && projectile.owner == Main.myPlayer)
                {
                    shootCount += 5;
                    canShootAgain = false;
                    projectile.direction = 1;
                    if (Main.MouseWorld.X < projectile.Center.X)
                    {
                        projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

                    Vector2 shootVel = Main.MouseWorld - armPosition;
                    shootVel.Normalize();
                    shootVel *= 12f;
                    int proj = Projectile.NewProjectile(armPosition, shootVel, ProjectileID.Bullet, (int)(altDamage * mPlayer.standDamageBoosts), 2f, projectile.owner);
                    Main.projectile[proj].netUpdate = true;
                    projectile.netUpdate = true;
                    projectile.velocity -= shootVel * 0.02f;
                    Main.PlaySound(SoundID.Item41, projectile.Center);
                }
                if (!Main.mouseRight && projectile.owner == Main.myPlayer)
                {
                    canShootAgain = true;
                }
                if (SpecialKeyPressedNoCooldown())
                {
                    sleepingGasTimer = 30 * 60;
                    sleepingGasFormPosition = projectile.Center;
                    player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(60));
                }

                if (SecondSpecialKeyPressedNoCooldown() && shootCount <= 0)
                {
                    shootCount += 30;
                    remoteControlled = false;
                }
            }

            if (mPlayer.standAutoMode)
            {
                BasicPunchAI();
            }

            if (sleepingGasTimer != 0)
            {
                sleepingGasTimer--;
                for (int i = 0; i < Main.rand.Next(1, 5); i++)
                {
                    Vector2 dustPosition = sleepingGasFormPosition + new Vector2(Main.rand.NextFloat(-SleepingGasEffectRadius, SleepingGasEffectRadius), Main.rand.NextFloat(-SleepingGasEffectRadius, SleepingGasEffectRadius));
                    if (Vector2.Distance(sleepingGasFormPosition, dustPosition) > SleepingGasEffectRadius)
                        continue;

                    int dustIndex = Dust.NewDust(dustPosition, 1, 1, 63, Scale: 1.6f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].fadeIn = 2f;
                    if (Main.rand.Next(0, 7 + 1) != 0)
                        Main.dust[dustIndex].noLight = true;
                }
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active && npc.Distance(sleepingGasFormPosition) <= SleepingGasEffectRadius)
                    {
                        float slowBonus = 0.25f;
                        if (npc.boss)
                            slowBonus = 0f;

                        npc.velocity.X *= 0.5f - slowBonus;
                        if (npc.noGravity)
                            npc.velocity.Y *= 0.5f - slowBonus;
                    }
                }
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        Player otherPlayer = Main.player[p];
                        if (otherPlayer.whoAmI == player.whoAmI)
                            continue;

                        if (otherPlayer.active && otherPlayer.team != player.team && otherPlayer.Distance(sleepingGasFormPosition) <= SleepingGasEffectRadius)
                            otherPlayer.velocity.X *= 0.5f;
                    }
                }
            }
        }

        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            if (remoteControlled)
            {
                Texture2D armTexture = mod.GetTexture("Projectiles/PlayerStands/Whitesnake/Whitesnake_Arm");
                Vector2 armOrigin = new Vector2(4f, 12f);
                int armFrameHeight = 16;
                Rectangle armSourceRect = new Rectangle(0, armFrame * armFrameHeight, 56, armFrameHeight);
                Color armColor = Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16);
                SpriteEffects armEffect = SpriteEffects.None;
                if (projectile.spriteDirection == -1)
                {
                    armEffect = SpriteEffects.FlipVertically;
                }

                spriteBatch.Draw(armTexture, armPosition - Main.screenPosition, armSourceRect, armColor, armRotation, armOrigin, 1f, armEffect, 0f);
            }
            return true;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(stealFrames);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            stealFrames = reader.ReadBoolean();
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
            if (gunRevealFrames)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                remoteControlFrames = false;
                waitingForEnemyFrames = false;
                PlayAnimation("GunReveal");
            }
            if (remoteControlFrames)
            {
                normalFrames = false;
                attackFrames = false;
                waitingForEnemyFrames = false;
                PlayAnimation("RemoteControl");
            }
            if (waitingForEnemyFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Steal");
                projectile.frame = 0;
            }
            if (stealFrames)
            {
                normalFrames = false;
                attackFrames = false;
                waitingForEnemyFrames = false;
                PlayAnimation("Steal");
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void AnimationCompleted(string animationName)
        {
            if (animationName == "GunReveal")
            {
                remoteControlled = true;
                gunRevealFrames = false;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/Whitesnake/Whitesnake_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 3, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 5, 10, true);
            }
            if (animationName == "GunReveal")
            {
                AnimateStand(animationName, 5, 3, false);
            }
            if (animationName == "RemoteControl")
            {
                AnimateStand(animationName, 1, 15, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 10, true);
            }
            if (animationName == "Steal")
            {
                AnimateStand(animationName, 7, 15, false);
            }
        }
    }
}