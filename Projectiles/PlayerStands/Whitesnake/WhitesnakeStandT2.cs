using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Whitesnake
{
    public class WhitesnakeStandT2 : StandClass
    {
        public override int punchDamage => 38;
        public override int altDamage => 25;
        public override int punchTime => 13;
        public override int halfStandHeight => 32;
        public override float fistWhoAmI => 9f;
        public override int standType => 1;
        public override int standOffset => -20;
        public override float maxDistance => 147f;      //1.5x the normal range cause Whitesnake is considered a long-range stand with melee capabilities
        public override string poseSoundName => "YouWereTwoSecondsTooLate";
        public override string spawnSoundName => "Whitesnake";

        private const float RemoteControlMaxDistance = 40f * 16f;

        private int updateTimer = 0;
        private bool remoteControlled = false;
        private bool gunRevealFrames = false;
        private bool remoteControlFrames = false;
        private int armFrame = 0;
        private float armRotation = 0;
        private float floatTimer = 0;
        private bool canShootAgain = false;
        private Vector2 armPosition;

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
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
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
                if (!attackFrames && !gunRevealFrames)
                {
                    if (!secondaryAbilityFrames)
                        StayBehind();
                    else
                        GoInFront();
                }
                if (SecondSpecialKeyPressedNoCooldown() && shootCount <= 0 && !gunRevealFrames)
                {
                    shootCount += 15;
                    attackFrames = false;
                    normalFrames = false;
                    secondaryAbilityFrames = false;
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
                    armPosition -= new Vector2(14f, 8f);
                }
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
                    projectile.velocity.X = moveVelocity.X * 4f;
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
                PlayAnimation("GunReveal");
            }
            if (remoteControlFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("RemoteControl");
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
        }
    }
}