using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Whitesnake
{
    public class WhitesnakeStandT3 : StandClass
    {
        public override int PunchDamage => 69;
        public override int AltDamage => 63;
        public override int PunchTime => 12;
        public override int HalfStandHeight => 44;
        public override int FistWhoAmI => 9;
        public override int TierNumber => 3;
        public override StandAttackType StandType => StandAttackType.Melee;
        public override int StandOffset => 22;
        public override float MaxDistance => 148f;      //1.5x the normal range cause Whitesnake is considered a long-range stand with melee capabilities
        public override string PoseSoundName => "YouWereTwoSecondsTooLate";
        public override string SpawnSoundName => "Whitesnake";
        public override bool CanUseSaladDye => true;

        private const float RemoteControlMaxDistance = 50f * 16f;
        private const float SleepingGasEffectRadius = 10f * 16f;

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
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            remoteControlFrames = remoteControlled;
            if (!mPlayer.standAutoMode && !remoteControlled)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !secondaryAbilityFrames && !stealFrames)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (Main.mouseRight && shootCount <= 0 && !secondaryAbilityFrames && Projectile.owner == Main.myPlayer)
                {
                    Projectile.frame = 0;
                    secondaryAbilityFrames = true;
                }
                if (secondaryAbilityFrames)
                {
                    Main.mouseLeft = false;
                    if (Projectile.frame >= 4)
                    {
                        shootCount += 120;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 8f;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<MeltYourHeart>(), (int)(AltDamage * mPlayer.standDamageBoosts), 2f, Projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
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
                if (SpecialKeyCurrent() && Projectile.owner == Main.myPlayer && shootCount <= 0 && !stealFrames)
                {
                    Projectile.velocity = Main.MouseWorld - Projectile.position;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 5f;

                    float mouseDistance = Vector2.Distance(Main.MouseWorld, Projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        Projectile.velocity = player.velocity + Projectile.velocity;
                    }
                    if (mouseDistance <= 40f)
                    {
                        Projectile.velocity = Vector2.Zero;
                    }
                    waitingForEnemyFrames = true;
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active)
                        {
                            if (Projectile.Distance(npc.Center) <= 30f && !npc.boss && !npc.immortal && !npc.hide)
                            {
                                Projectile.ai[0] = npc.whoAmI;
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
                                if (Projectile.Distance(otherPlayer.Center) <= 30f & otherPlayer.team != player.team && otherPlayer.whoAmI != player.whoAmI)
                                {
                                    Projectile.ai[1] = otherPlayer.whoAmI;
                                    stealFrames = true;
                                }
                            }
                        }
                    }
                    LimitDistance();
                }
                if (stealFrames && Projectile.ai[0] != -1f)
                {
                    Projectile.velocity = Vector2.Zero;
                    NPC npc = Main.npc[(int)Projectile.ai[0]];
                    npc.direction = -Projectile.direction;
                    npc.position = Projectile.position + new Vector2(-6f * Projectile.direction, -2f - npc.height / 3f);
                    npc.velocity = Vector2.Zero;
                    if (Projectile.frame == 4)
                    {
                        npc.AddBuff(ModContent.BuffType<Stolen>(), 30 * 60);
                    }
                    if (Projectile.frame == 6)
                    {
                        stealFrames = false;
                        Projectile.ai[0] = -1f;
                        shootCount += 60;
                    }
                    if (!npc.active)
                    {
                        stealFrames = false;
                        Projectile.ai[0] = -1f;
                        shootCount += 30;
                    }
                    LimitDistance();
                }
                if (stealFrames && Projectile.ai[1] != -1f)
                {
                    Projectile.velocity = Vector2.Zero;
                    Player otherPlayer = Main.player[(int)Projectile.ai[1]];
                    otherPlayer.direction = -Projectile.direction;
                    otherPlayer.position = Projectile.position + new Vector2(-6f * Projectile.direction, -2f - otherPlayer.height / 3f);
                    otherPlayer.velocity = Vector2.Zero;
                    if (Projectile.frame == 4)      //this is the frame where the disc has just been stolen
                    {
                        otherPlayer.AddBuff(ModContent.BuffType<Stolen>(), 30 * 60);
                    }
                    if (Projectile.frame == 6)      //anim ended
                    {
                        stealFrames = false;
                        Projectile.ai[1] = -1f;
                        shootCount += 60;
                    }
                    if (!otherPlayer.active)
                    {
                        stealFrames = false;
                        Projectile.ai[1] = -1f;
                        shootCount += 30;
                    }
                    LimitDistance();
                }
                if (!SpecialKeyCurrent() && ((stealFrames || waitingForEnemyFrames) || Projectile.ai[1] != -1f))
                {
                    stealFrames = false;
                    waitingForEnemyFrames = false;
                    Projectile.ai[0] = -1f;
                    Projectile.ai[1] = -1f;
                    shootCount += 30;
                }

                if (SecondSpecialKeyPressedNoCooldown() && shootCount <= 0 && !gunRevealFrames && !stealFrames)
                {
                    shootCount += 15;
                    attackFrames = false;
                    idleFrames = false;
                    secondaryAbilityFrames = false;
                    waitingForEnemyFrames = false;
                    Projectile.frame = 0;
                    Projectile.frameCounter = 0;
                    gunRevealFrames = true;
                }
            }
            if (!mPlayer.standAutoMode && remoteControlled)
            {
                mPlayer.standRemoteMode = true;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);

                floatTimer += 0.06f;
                armRotation = (Main.MouseWorld - Projectile.Center).ToRotation();
                armPosition = Projectile.Center + new Vector2(0f, -4f);
                if (Projectile.spriteDirection == -1)
                    armPosition += new Vector2(2f, -8f);

                HandleDrawOffsets();
                if (shootCount > 0)
                    armFrame = 1;
                else
                    armFrame = 0;

                bool aboveTile = Collision.SolidTiles((int)Projectile.Center.X / 16, (int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, (int)(Projectile.Center.Y / 16) + 4);
                if (aboveTile)
                {
                    Projectile.velocity.Y = (float)Math.Sin(floatTimer) / 5f;
                }
                else
                {
                    if (Projectile.velocity.Y < 6f)
                    {
                        Projectile.velocity.Y += 0.2f;
                    }
                    if (Vector2.Distance(Projectile.Center, player.Center) >= RemoteControlMaxDistance)
                    {
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 0.8f;
                    }
                }

                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    Vector2 moveVelocity = Main.MouseWorld - Projectile.Center;
                    moveVelocity.Normalize();
                    Projectile.velocity.X = moveVelocity.X * 4.5f;
                    if (aboveTile)
                        Projectile.velocity.Y += moveVelocity.Y * 2f;

                    Projectile.direction = 1;
                    if (Main.MouseWorld.X < Projectile.Center.X)
                        Projectile.direction = -1;

                    Projectile.spriteDirection = Projectile.direction;

                    if (Vector2.Distance(Projectile.Center, player.Center) >= RemoteControlMaxDistance)
                    {
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 0.8f;
                    }
                    Projectile.netUpdate = true;
                }
                if (!Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    Projectile.velocity.X *= 0.78f;
                    Projectile.netUpdate = true;
                }
                if (Main.mouseRight && canShootAgain && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                {
                    shootCount += 5;
                    canShootAgain = false;
                    Projectile.direction = 1;
                    if (Main.MouseWorld.X < Projectile.Center.X)
                    {
                        Projectile.direction = -1;
                    }
                    Projectile.spriteDirection = Projectile.direction;

                    Vector2 bulletSpawnPosition = armPosition + new Vector2(0f, -4f);
                    Vector2 shootVel = Main.MouseWorld - armPosition;
                    shootVel.Normalize();
                    shootVel *= 12f;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), bulletSpawnPosition, shootVel, ModContent.ProjectileType<StandBullet>(), (int)(AltDamage * mPlayer.standDamageBoosts), 2f, Projectile.owner);
                    Main.projectile[proj].netUpdate = true;
                    Projectile.netUpdate = true;
                    Projectile.velocity -= shootVel * 0.02f;
                    SoundEngine.PlaySound(SoundID.Item41, Projectile.Center);
                }
                if (!Main.mouseRight && Projectile.owner == Main.myPlayer)
                {
                    canShootAgain = true;
                }
                if (SpecialKeyPressedNoCooldown())
                {
                    sleepingGasTimer = 30 * 60;
                    sleepingGasFormPosition = Projectile.Center;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(60));
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
                if (MyPlayer.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
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

        public override bool PreDrawExtras()
        {
            if (remoteControlled)
            {
                Texture2D armTexture;
                if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().currentTextureDye == MyPlayer.StandTextureDye.Salad)
                    armTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/Whitesnake/Salad/Whitesnake_Arm");
                else
                    armTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/Whitesnake/Whitesnake_Arm");
                
                Vector2 armOrigin = new Vector2(4f, 12f);
                int armFrameHeight = 16;
                Rectangle armSourceRect = new Rectangle(0, armFrame * armFrameHeight, 56, armFrameHeight);
                Color armColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16);
                SpriteEffects armEffect = SpriteEffects.None;
                if (Projectile.spriteDirection == -1)
                {
                    armEffect = SpriteEffects.FlipVertically;
                }

                Main.EntitySpriteDraw(armTexture, armPosition - Main.screenPosition, armSourceRect, armColor, armRotation, armOrigin, 1f, armEffect, 0);
            }
            return true;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(stealFrames);
            writer.Write(remoteControlled);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            stealFrames = reader.ReadBoolean();
            remoteControlled = reader.ReadBoolean();
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
            if (gunRevealFrames)
            {
                idleFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                remoteControlFrames = false;
                waitingForEnemyFrames = false;
                PlayAnimation("GunReveal");
            }
            if (remoteControlFrames)
            {
                idleFrames = false;
                attackFrames = false;
                waitingForEnemyFrames = false;
                PlayAnimation("RemoteControl");
            }
            if (waitingForEnemyFrames)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Steal");
                Projectile.frame = 0;
            }
            if (stealFrames)
            {
                idleFrames = false;
                attackFrames = false;
                waitingForEnemyFrames = false;
                PlayAnimation("Steal");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().posing)
            {
                idleFrames = false;
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
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/Whitesnake", "/Whitesnake_" + animationName);

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