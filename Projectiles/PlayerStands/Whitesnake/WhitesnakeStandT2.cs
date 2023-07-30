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
    public class WhitesnakeStandT2 : StandClass
    {
        public override int PunchDamage => 38;
        public override int AltDamage => 25;
        public override int PunchTime => 13;
        public override int HalfStandHeight => 32;
        public override int FistWhoAmI => 9;
        public override int TierNumber => 2;
        public override Vector2 StandOffset => new Vector2(11, 0);
        public override float MaxDistance => 148f;      //1.5x the normal range cause Whitesnake is considered a long-range stand with melee capabilities
        public override string PoseSoundName => "YouWereTwoSecondsTooLate";
        public override string SpawnSoundName => "Whitesnake";
        public override bool CanUseSaladDye => true;
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        private const float RemoteControlMaxDistance = 40f * 16f;
        private readonly Vector2 ArmOrigin = new Vector2(4f, 12f);

        private bool revealingGun = false;
        private bool remoteControlFrames = false;
        private int armFrame = 0;
        private int armFrameCounter = 0;
        private float armRotation = 0;
        private float floatTimer = 0;
        private bool canShootAgain = false;
        private Vector2 armPosition;
        private Vector2 armOffset;

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

            remoteControlFrames = mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote;
            if (armFrame == 1)
                Lighting.AddLight(Projectile.position, 0);

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !secondaryAbility)
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
                        secondaryAbility = false;
                    }
                }
                if (!attacking && !revealingGun)
                {
                    if (!secondaryAbility)
                        StayBehind();
                    else
                        GoInFront();
                }
                if (SecondSpecialKeyPressed(false) && shootCount <= 0 && !revealingGun)
                {
                    shootCount += 15;
                    secondaryAbility = false;
                    Projectile.frame = 0;
                    Projectile.frameCounter = 0;
                    revealingGun = true;
                    currentAnimationState = AnimationState.GunReveal;
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote)
            {
                mPlayer.standControlStyle = MyPlayer.StandControlStyle.Remote;
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
                        Projectile.velocity.Y += 0.2f;

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
                        Projectile.velocity.X = moveVelocity.X * 4f;
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
                        shootCount += 20;
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
            writer.Write(revealingGun);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            revealingGun = reader.ReadBoolean();
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