using JoJoStands.Buffs.Debuffs;
using JoJoStands.Networking;
using JoJoStands.NPCs;
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
        public override Vector2 StandOffset => new Vector2(11, 0);
        public override float MaxDistance => 148f;      //1.5x the normal range cause Whitesnake is considered a long-range stand with melee capabilities
        public override string PoseSoundName => "YouWereTwoSecondsTooLate";
        public override string SpawnSoundName => "Whitesnake";
        public override bool CanUseSaladDye => true;
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        private const float RemoteControlMaxDistance = 50f * 16f;
        private const float SleepingGasEffectRadius = 10f * 16f;
        private readonly Vector2 ArmOrigin = new Vector2(4f, 12f);

        private bool stealFrames = false;
        private bool waitingForStealEnemy = false;
        private int waitingForEnemyFramesButInt = 0;
        private bool revealingGun = false;
        private int armFrame = 0;
        private int armFrameCounter = 0;
        private float armRotation = 0;
        private float floatTimer = 0;
        private bool canShootAgain = false;
        private Vector2 armPosition;
        private Vector2 armOffset;
        private int sleepingGasTimer = 0;
        private Vector2 sleepingGasPosition;

        public new enum AnimationState
        {
            Idle,
            Attack,
            Secondary,
            RemoteControl,
            GunReveal,
            Steal,
            Pose
        }

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;
            if (armFrameCounter > 0)
            {
                armFrameCounter--;
                if (armFrameCounter <= 0)
                    armFrame = 0;
            }

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (!waitingForStealEnemy && waitingForEnemyFramesButInt > 0)
                waitingForEnemyFramesButInt--;

            if (waitingForEnemyFramesButInt > 0)
            {
                if (mouseX > Projectile.position.X)
                    Projectile.spriteDirection = 1;
                else
                    Projectile.spriteDirection = -1;
            }

            if (armFrame == 1)
                Lighting.AddLight(Projectile.position, 0);

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !secondaryAbility && !stealFrames)
                    {
                        currentAnimationState = AnimationState.Attack;
                        Punch();
                    }
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }
                    if (Main.mouseRight && shootCount <= 0 && !secondaryAbility)
                    {
                        Projectile.frame = 0;
                        secondaryAbility = true;
                    }
                }
                if (secondaryAbility)
                {
                    player.direction = Main.MouseWorld.X > player.Center.X ? 1 : -1;
                    currentAnimationState = AnimationState.Secondary;
                    if (Projectile.frame >= 4 && shootCount <= 0)
                    {
                        shootCount += 30;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= 10f;
                        int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<MeltYourHeart>(), (int)(AltDamage * mPlayer.standDamageBoosts), 2f, Projectile.owner);
                        Main.projectile[projIndex].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                }
                if (!attacking && !stealFrames && !waitingForStealEnemy && !revealingGun && waitingForEnemyFramesButInt == 0)
                {
                    if (!secondaryAbility)
                        StayBehind();
                    else
                        GoInFront();
                }
                if (SpecialKeyCurrent() && shootCount <= 0 && !stealFrames)
                {
                    waitingForEnemyFramesButInt = 10;
                    Projectile.velocity = Main.MouseWorld - Projectile.position;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 5f;
                    Projectile.netUpdate = true;

                    float mouseDistance = Vector2.Distance(Main.MouseWorld, Projectile.Center);
                    if (mouseDistance > 40f)
                        Projectile.velocity = player.velocity + Projectile.velocity;
                    else
                        Projectile.velocity = Vector2.Zero;
                    waitingForStealEnemy = true;
                    currentAnimationState = AnimationState.Steal;
                    Projectile.frame = 0;
                    Projectile.frameCounter = 0;
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active)
                        {
                            if (Projectile.Distance(npc.Center) <= 30f && !npc.immortal && !npc.hide)
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
                    currentAnimationState = AnimationState.Steal;
                    Projectile.velocity = Vector2.Zero;
                    NPC npc = Main.npc[(int)Projectile.ai[0]];
                    npc.direction = -Projectile.direction;
                    npc.position = Projectile.position + new Vector2(-6f * Projectile.direction, -2f - npc.height / 3f);
                    npc.velocity = Vector2.Zero;
                    if (Projectile.frame == 4)
                    {
                        npc.AddBuff(ModContent.BuffType<Stolen>(), 30 * 60);
                        npc.GetGlobalNPC<JoJoGlobalNPC>().whitesnakeDISCImmune += 1;
                        SyncCall.SyncStandEffectInfo(player.whoAmI, npc.whoAmI, 9);
                        Projectile.frame += 1;
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
                    currentAnimationState = AnimationState.Steal;
                    Projectile.velocity = Vector2.Zero;
                    Player otherPlayer = Main.player[(int)Projectile.ai[1]];
                    otherPlayer.direction = -Projectile.direction;
                    otherPlayer.position = Projectile.position + new Vector2(-6f * Projectile.direction, -2f - otherPlayer.height / 3f);
                    otherPlayer.velocity = Vector2.Zero;
                    if (Projectile.frame == 4)      //this is the frame where the disc has just been stolen
                    {
                        otherPlayer.AddBuff(ModContent.BuffType<Stolen>(), 30 * 60);
                        SyncCall.SyncOtherPlayerDebuff(player.whoAmI, otherPlayer.whoAmI, ModContent.BuffType<Stolen>(), 30 * 60);
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
                if (!SpecialKeyCurrent() && stealFrames || !SpecialKeyCurrent() && waitingForStealEnemy || Projectile.ai[1] != -1f)
                {
                    stealFrames = false;
                    waitingForStealEnemy = false;
                    Projectile.ai[0] = -1f;
                    Projectile.ai[1] = -1f;
                    shootCount += 30;
                    currentAnimationState = AnimationState.Idle;
                }

                if (SecondSpecialKeyPressed(false) && shootCount <= 0 && !revealingGun && !stealFrames)
                {
                    secondaryAbility = false;
                    waitingForStealEnemy = false;
                    Projectile.frame = 0;
                    Projectile.frameCounter = 0;
                    revealingGun = true;
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote)
            {
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
                if (mouseX > Projectile.Center.X)
                    Projectile.direction = 1;
                else
                    Projectile.direction = -1;
                Projectile.spriteDirection = Projectile.direction;
                floatTimer += 0.06f;
                currentAnimationState = AnimationState.RemoteControl;
                armRotation = (new Vector2(mouseX, mouseY) - Projectile.Center).ToRotation();
                armPosition = Projectile.Center + new Vector2(0f, -4f);
                armOffset = Vector2.Zero;
                if (Projectile.direction == -1)
                    armOffset = new Vector2(2f, -8f);
                if (mPlayer.posing)
                    canShootAgain = false;

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

                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft)
                    {
                        Vector2 moveVelocity = Main.MouseWorld - Projectile.Center;
                        moveVelocity.Normalize();
                        Projectile.velocity.X = moveVelocity.X * 4.5f;
                        if (aboveTile)
                            Projectile.velocity.Y += moveVelocity.Y * 2f;

                        if (Vector2.Distance(Projectile.Center, player.Center) >= RemoteControlMaxDistance)
                        {
                            Projectile.velocity = player.Center - Projectile.Center;
                            Projectile.velocity.Normalize();
                            Projectile.velocity *= 0.8f;
                        }
                        Projectile.netUpdate = true;
                    }
                    else
                    {
                        Projectile.velocity.X *= 0.78f;
                        Projectile.netUpdate = true;
                    }
                    if (Main.mouseRight && canShootAgain && shootCount <= 0)
                    {
                        armFrame = 1;
                        shootCount += 15;
                        armFrameCounter += 3;
                        canShootAgain = false;
                        Projectile.direction = 1;
                        if (mouseX < Projectile.Center.X)
                            Projectile.direction = -1;
                        Projectile.spriteDirection = Projectile.direction;
                        Vector2 shootOffset = new Vector2(2f, -2f);
                        if (Projectile.direction == 1)
                            shootOffset.Y = -8f;

                        Vector2 bulletSpawnPosition = armPosition + armOffset + shootOffset + (armRotation.ToRotationVector2() * 12f);
                        Vector2 shootVel = Main.MouseWorld - bulletSpawnPosition;
                        shootVel.Normalize();
                        shootVel *= 12f;
                        int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), bulletSpawnPosition, shootVel, ModContent.ProjectileType<StandBullet>(), (int)(AltDamage * mPlayer.standDamageBoosts), 2f, Projectile.owner);
                        Main.projectile[projIndex].netUpdate = true;
                        Projectile.netUpdate = true;
                        Projectile.velocity -= shootVel * 0.02f;
                        SoundEngine.PlaySound(SoundID.Item41, Projectile.Center);
                    }
                    if (!Main.mouseRight)
                        canShootAgain = true;
                }
                if (SpecialKeyPressed())
                {
                    sleepingGasTimer = 30 * 60;
                    sleepingGasPosition = Projectile.Center;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(60));
                }

                if (SecondSpecialKeyPressed(false) && shootCount <= 0)
                {
                    shootCount += 30;
                    mPlayer.standControlStyle = MyPlayer.StandControlStyle.Manual;
                }
            }

            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }

            if (sleepingGasTimer != 0)
            {
                sleepingGasTimer--;
                for (int i = 0; i < Main.rand.Next(1, 5); i++)
                {
                    Vector2 dustPosition = sleepingGasPosition + new Vector2(Main.rand.NextFloat(-SleepingGasEffectRadius, SleepingGasEffectRadius), Main.rand.NextFloat(-SleepingGasEffectRadius, SleepingGasEffectRadius));
                    if (Vector2.Distance(sleepingGasPosition, dustPosition) > SleepingGasEffectRadius)
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
                    if (npc.active && npc.Distance(sleepingGasPosition) <= SleepingGasEffectRadius)
                    {
                        float slowBonus = 0.25f;
                        if (npc.boss)
                            slowBonus = 0f;

                        npc.velocity.X *= 0.5f - slowBonus;
                        if (npc.noGravity)
                            npc.velocity.Y *= 0.5f - slowBonus;
                    }
                }
                if (JoJoStands.StandPvPMode && Main.netMode != NetmodeID.SinglePlayer)
                {
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        Player otherPlayer = Main.player[p];
                        if (otherPlayer.whoAmI == player.whoAmI)
                            continue;

                        if (otherPlayer.active && otherPlayer.team != player.team && otherPlayer.Distance(sleepingGasPosition) <= SleepingGasEffectRadius)
                            otherPlayer.velocity.X *= 0.5f;
                    }
                }
            }
            if (revealingGun)
                currentAnimationState = AnimationState.GunReveal;
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }


        public override bool PreDrawExtras()
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote && !mPlayer.posing)
            {
                Texture2D armTexture;
                if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().currentTextureDye == MyPlayer.StandTextureDye.Salad)
                    armTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/Whitesnake/Salad/Whitesnake_Arm");
                else
                    armTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/Whitesnake/Whitesnake_Arm");
                int armFrameHeight = 16;
                Rectangle armSourceRect = new Rectangle(0, armFrame * armFrameHeight, 56, armFrameHeight);
                Color armColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16);
                SpriteEffects armEffect = SpriteEffects.None;
                if (Projectile.spriteDirection == -1)
                    armEffect = SpriteEffects.FlipVertically;

                Main.EntitySpriteDraw(armTexture, armPosition + armOffset + Projectile.velocity - Main.screenPosition, armSourceRect, armColor, armRotation, ArmOrigin, 1f, armEffect, 0);
            }
            return true;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(stealFrames);
            writer.Write(waitingForStealEnemy);
            writer.Write(revealingGun);

            writer.Write(waitingForEnemyFramesButInt);
            writer.Write(sleepingGasTimer);

            writer.Write(sleepingGasPosition.X);
            writer.Write(sleepingGasPosition.Y);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            stealFrames = reader.ReadBoolean();
            waitingForStealEnemy = reader.ReadBoolean();
            revealingGun = reader.ReadBoolean();

            waitingForEnemyFramesButInt = reader.ReadInt32();
            sleepingGasTimer = reader.ReadInt32();
            sleepingGasPosition = new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }

        public override byte SendAnimationState() => (byte)currentAnimationState;
        public override void ReceiveAnimationState(byte state) => currentAnimationState = (AnimationState)state;

        public override void SelectAnimation()
        {
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState;
                Projectile.netUpdate = true;
            }

            if (currentAnimationState == AnimationState.Idle)
                PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Attack)
                PlayAnimation("Attack");
            else if (currentAnimationState == AnimationState.Secondary)
                PlayAnimation("Secondary");
            else if (currentAnimationState == AnimationState.GunReveal)
                PlayAnimation("GunReveal");
            else if (currentAnimationState == AnimationState.RemoteControl)
                PlayAnimation("RemoteControl");
            else if (currentAnimationState == AnimationState.Steal)
                PlayAnimation("Steal");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void AnimationCompleted(string animationName)
        {
            if (animationName == "GunReveal")
            {
                revealingGun = false;
                Main.player[Projectile.owner].GetModPlayer<MyPlayer>().standControlStyle = MyPlayer.StandControlStyle.Remote;
            }
            else if (animationName == "Secondary")
            {
                secondaryAbility = false;
                currentAnimationState = AnimationState.Idle;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/Whitesnake", "Whitesnake_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 30, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 3, newPunchTime, true);
            else if (animationName == "Secondary")
                AnimateStand(animationName, 5, 4, false);
            else if (animationName == "GunReveal")
                AnimateStand(animationName, 5, 3, false);
            else if (animationName == "RemoteControl")
                AnimateStand(animationName, 1, 15, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 10, true);
            else if (animationName == "Steal")
                AnimateStand(animationName, 7, 15, false);
        }
    }
}