using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Aerosmith
{
    public class AerosmithStandT3 : StandClass
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/PlayerStands/Aerosmith/Aerosmith"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void ExtraSetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 40;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.netImportant = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 5;
            Projectile.ignoreWater = true;
        }

        public override float ProjectileSpeed => 12f;

        public override int ProjectileDamage => 63;
        public override int ShootTime => 10;      //+2 every tier
        public override int TierNumber => 3;
        public override StandAttackType StandType => StandAttackType.Ranged;
        public override string PoseSoundName => "VolareVia";
        public override string SpawnSoundName => "Aerosmith";
        public override bool CanUseRangeIndicators => false;

        private bool bombless = false;
        private bool fallingFromSpace = false;
        private bool remoteMode = false;
        private int leftClickCooldown = 0;
        private int accelerationTimer = 0;
        private SoundEffectInstance aerosmithWhirrSound;
        private const int AccelerationTime = (int)(1.5f * 60);
        private const float MaxFlightSpeed = 9f;
        private const float WhirrSoundDistance = 92 * 16f;
        private const float AerosmithHoverHeightOffset = 3.5f * 16f;
        private const float AutoModeDetectionDistance = 32f * 16f;

        public override void ExtraSpawnEffects()
        {
            aerosmithWhirrSound = AerosmithStandFinal.AerosmithWhirrSoundEffect.CreateInstance();
        }

        public override void AI()
        {
            SelectFrame();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;
            if (accelerationTimer > 0)
                accelerationTimer--;

            mPlayer.aerosmithWhoAmI = Projectile.whoAmI;
            newProjectileDamage = (int)(newProjectileDamage * MathHelper.Clamp(1f - (Projectile.Distance(player.Center) / (350f * 16f)), 0.5f, 1f));

            fallingFromSpace = Projectile.position.Y < (Main.worldSurface * 0.35) * 16f;
            if (fallingFromSpace)
            {
                Projectile.frameCounter = 0;
                Projectile.velocity.Y += 0.3f;
                Projectile.netUpdate = true;
            }
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 rota = Projectile.Center - Main.MouseWorld;
                Projectile.rotation = (-rota * Projectile.direction).ToRotation();
            }
            bombless = player.HasBuff(ModContent.BuffType<AbilityCooldown>());
            Projectile.tileCollide = true;

            if (Projectile.velocity.X > 0.5f)
                Projectile.spriteDirection = 1;
            else if (Projectile.velocity.X < -0.5f)
                Projectile.spriteDirection = -1;

            if (leftClickCooldown > 0)
                leftClickCooldown--;

            if (remoteMode)
            {
                player.aggro -= 1200;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
            }
            if (Projectile.owner != Main.myPlayer)
            {
                if (aerosmithWhirrSound == null)
                    aerosmithWhirrSound = AerosmithStandFinal.AerosmithWhirrSoundEffect.CreateInstance();
                else
                {
                    float playerDistance = Vector2.Distance(Main.player[Main.myPlayer].Center, Projectile.Center);
                    if (playerDistance <= WhirrSoundDistance && !SoundEngine.AreSoundsPaused)
                    {
                        aerosmithWhirrSound.Volume = (1f - Math.Clamp((playerDistance * 1.4f) / WhirrSoundDistance, 0f, 1f)) * 0.6f;
                        aerosmithWhirrSound.Volume *= Projectile.velocity.Length() / MaxFlightSpeed;
                        aerosmithWhirrSound.Volume *= Main.soundVolume;
                        aerosmithWhirrSound.Pitch = Math.Clamp((Main.player[Main.myPlayer].Center.X - Projectile.Center.X) / WhirrSoundDistance, -1f, 1f);
                        if (aerosmithWhirrSound.State != SoundState.Playing)
                            aerosmithWhirrSound.Play();
                    }
                    else
                    {
                        if (aerosmithWhirrSound.State != SoundState.Stopped)
                            aerosmithWhirrSound.Stop();
                    }
                }
            }
            else
            {
                if (!remoteMode)
                {
                    float playerDistance = Vector2.Distance(Main.player[Projectile.owner].Center, Projectile.Center);
                    if (playerDistance <= WhirrSoundDistance && !SoundEngine.AreSoundsPaused)
                    {
                        if (aerosmithWhirrSound.State != SoundState.Playing)
                            aerosmithWhirrSound.Play();

                        aerosmithWhirrSound.Volume = (1f - Math.Clamp(playerDistance / WhirrSoundDistance, 0f, 1f)) * 0.6f;
                        aerosmithWhirrSound.Volume *= Projectile.velocity.Length() / MaxFlightSpeed;
                        aerosmithWhirrSound.Volume *= Main.soundVolume;
                        aerosmithWhirrSound.Pitch = (1f - ((1f - Math.Clamp((playerDistance * 1.4f) / WhirrSoundDistance, 0.6f, 1f)) * 2f)) * 0.4f;
                        float xDifference = Main.player[Main.myPlayer].Center.X - Projectile.Center.X;
                        if ((int)xDifference == 0)
                            xDifference = 1;
                        int relativeDirection = (int)Math.Ceiling(xDifference * 100) / (int)(Math.Abs(xDifference * 100));
                        aerosmithWhirrSound.Pan = Math.Clamp(playerDistance / WhirrSoundDistance, -1f, 1f) * -relativeDirection;
                    }
                    else
                    {
                        if (aerosmithWhirrSound.State != SoundState.Stopped)
                            aerosmithWhirrSound.Stop();
                    }
                }
                else
                {
                    if (!SoundEngine.AreSoundsPaused)
                    {
                        aerosmithWhirrSound.Volume = (Math.Abs(Projectile.velocity.Length()) / MaxFlightSpeed) * 0.6f;
                        aerosmithWhirrSound.Volume *= Main.soundVolume;
                        aerosmithWhirrSound.Pitch = Math.Clamp(((Projectile.velocity.Length() * 2 - MaxFlightSpeed) / 2f) / MaxFlightSpeed, -0.4f, 0.3f);
                        if (aerosmithWhirrSound.State != SoundState.Playing)
                            aerosmithWhirrSound.Play();
                    }
                    else
                        aerosmithWhirrSound.Stop();
                }
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual || mPlayer.standControlStyle == MyPlayer.StandControlStyle.Remote)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    leftClickCooldown = 10;
                    float mouseDistance = Vector2.Distance(Main.MouseWorld, Projectile.Center);
                    Projectile.spriteDirection = Projectile.direction;
                    accelerationTimer += 2;
                    if (accelerationTimer >= AccelerationTime)
                        accelerationTimer = AccelerationTime;

                    if (mouseDistance > 40f)
                    {
                        Projectile.velocity = Main.MouseWorld - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= MaxFlightSpeed + player.moveSpeed;
                        Projectile.velocity *= accelerationTimer / (float)AccelerationTime;
                    }
                    else
                    {
                        Projectile.velocity = Main.MouseWorld - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= (mouseDistance * (MaxFlightSpeed + player.moveSpeed)) / 40f;
                        Projectile.velocity *= accelerationTimer / (float)AccelerationTime;
                    }
                    Projectile.netUpdate = true;
                }
                else
                {
                    Projectile.rotation = (Projectile.velocity * Projectile.direction).ToRotation();
                    if (Projectile.Distance(player.Center) > 80f)
                    {
                        Projectile.velocity *= 0.95f;
                        Projectile.netUpdate = true;
                    }
                }
                if (remoteMode && Math.Abs(Projectile.velocity.X) >= 8f)
                {
                    int amountOfCloudDusts = Main.rand.Next(1, 3 + 1);
                    for (int i = 0; i < amountOfCloudDusts; i++)
                    {
                        float speedScale = Main.rand.Next(-24, -16 + 1) / 10f;
                        speedScale *= 4f;
                        float dustScale = Main.rand.Next(8, 12 + 1) / 10f;
                        int dustIndex = Dust.NewDust(Projectile.Center - (Main.ScreenSize.ToVector2() / 2f) + new Vector2(12 * 16f * Projectile.direction, 0f), Main.screenWidth, Main.screenHeight, DustID.Cloud, Projectile.velocity.X * speedScale, Alpha: 120, Scale: dustScale);
                        Main.dust[dustIndex].noGravity = true;

                    }
                }
                if (Main.mouseRight && Projectile.owner == Main.myPlayer)
                {
                    if (mouseX > Projectile.Center.X)
                        Projectile.spriteDirection = 1;
                    else
                        Projectile.spriteDirection = -1;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= 32f;
                        int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StandBullet>(), newProjectileDamage, 3f, Projectile.owner);
                        Main.projectile[projIndex].netUpdate = true;
                        SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                        int amountOfShootDusts = Main.rand.Next(1, 2 + 1);
                        for (int i = 0; i < amountOfShootDusts; i++)
                        {
                            float dustScale = Main.rand.Next(8, 12 + 1) / 10f;
                            if (Main.rand.Next(0, 1 + 1) == 0)
                            {
                                int dustIndex = Dust.NewDust(Projectile.Center - Vector2.One * 3f, 2, 2, DustID.Torch, Scale: dustScale);
                                Main.dust[dustIndex].velocity = shootVel * 1.1f * (Main.rand.Next(8, 12 + 1) / 10f);
                                Main.dust[dustIndex].noGravity = true;
                            }
                            else
                            {
                                int dustIndex = Dust.NewDust(Projectile.Center - Vector2.One * 3f, 2, 2, DustID.Smoke, shootVel.X * 0.5f, shootVel.Y * 0.5f, Scale: dustScale);
                                Main.dust[dustIndex].noGravity = true;
                            }
                        }
                    }
                }
                if (!remoteMode && leftClickCooldown == 0)
                {
                    if (Projectile.Distance(player.Center) < 12 * 16f)
                    {
                        if (Projectile.Center.X >= player.Center.X + (10 * 16f) || WorldGen.SolidTile((int)(Projectile.Center.X / 16) + 2, (int)(Projectile.Center.Y / 16f) + 1))
                        {
                            Projectile.velocity.X = -2.4f;
                            Projectile.spriteDirection = Projectile.direction = -1;
                            Projectile.netUpdate = true;
                        }
                        if (Projectile.Center.X <= player.Center.X - (10 * 16f) || WorldGen.SolidTile((int)(Projectile.Center.X / 16) - 2, (int)(Projectile.Center.Y / 16f) + 1))
                        {
                            Projectile.velocity.X = 2.4f;
                            Projectile.spriteDirection = Projectile.direction = 1;
                            Projectile.netUpdate = true;
                        }
                        if (Math.Abs(Projectile.velocity.X) > 2.4f)
                            Projectile.velocity.X *= 0.95f;

                        if (Projectile.Center.Y > player.Center.Y - AerosmithHoverHeightOffset - 4 && Projectile.Center.Y < player.Center.Y - AerosmithHoverHeightOffset + 4)
                        {
                            Projectile.velocity.Y = 0f;
                            Projectile.netUpdate = true;
                        }
                        else
                        {
                            Vector2 velocity = (player.Center + new Vector2(0f, -AerosmithHoverHeightOffset)) - Projectile.Center;
                            velocity.Normalize();
                            velocity.Y *= 6f;
                            Projectile.velocity.Y = velocity.Y * ((Vector2.Distance(player.Center + new Vector2(0f, -AerosmithHoverHeightOffset), Projectile.Center)) / (12 * 16f));
                        }
                    }
                    else if (Projectile.Distance(player.Center) > 16 * 16f)
                    {
                        Projectile.tileCollide = false;
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= MaxFlightSpeed + player.moveSpeed;
                        Projectile.netUpdate = true;
                    }
                    else
                    {
                        Projectile.tileCollide = false;
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= player.moveSpeed + 6f;
                        Projectile.netUpdate = true;
                    }
                }
                if (SpecialKeyPressed(false))
                {
                    remoteMode = !remoteMode;
                    if (remoteMode)
                    {
                        Main.NewText("Remote Mode: Active");
                        mPlayer.standControlStyle = MyPlayer.StandControlStyle.Remote;
                    }
                    else
                    {
                        Main.NewText("Remote Mode: Disabled");
                        mPlayer.standControlStyle = MyPlayer.StandControlStyle.Manual;
                    }
                }
                if (SecondSpecialKeyPressed(false) && !bombless)
                {
                    shootCount += newShootTime;
                    Projectile.frame = 2;
                    int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<AerosmithBomb>(), 0, 3f, Projectile.owner, 568 * (float)mPlayer.standDamageBoosts);
                    Main.projectile[projIndex].netUpdate = true;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(5));
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                remoteMode = false;
                Projectile.rotation = (Projectile.velocity * Projectile.direction).ToRotation();
                NPC target = FindNearestTarget(AutoModeDetectionDistance);
                if (target == null)
                {
                    if (Projectile.Distance(player.Center) < 12 * 16f)
                    {
                        if (Projectile.Center.X >= player.Center.X + (10 * 16f) || WorldGen.SolidTile((int)(Projectile.Center.X / 16) + 2, (int)(Projectile.Center.Y / 16f) + 1))
                        {
                            Projectile.velocity.X = -2.4f;
                            Projectile.spriteDirection = Projectile.direction = -1;
                            Projectile.netUpdate = true;
                        }
                        if (Projectile.Center.X <= player.Center.X - (10 * 16f) || WorldGen.SolidTile((int)(Projectile.Center.X / 16) - 2, (int)(Projectile.Center.Y / 16f) + 1))
                        {
                            Projectile.velocity.X = 2.4f;
                            Projectile.spriteDirection = Projectile.direction = 1;
                            Projectile.netUpdate = true;
                        }
                        if (Math.Abs(Projectile.velocity.X) > 2.4f)
                            Projectile.velocity.X *= 0.95f;

                        if (Projectile.Center.Y > player.Center.Y - AerosmithHoverHeightOffset - 16 - 4 && Projectile.Center.Y < player.Center.Y - AerosmithHoverHeightOffset - 16 + 4)
                        {
                            Projectile.velocity.Y = 0f;
                            Projectile.netUpdate = true;
                        }
                        else
                        {
                            Vector2 velocity = (player.Center + new Vector2(0f, -AerosmithHoverHeightOffset - 16)) - Projectile.Center;
                            velocity.Normalize();
                            velocity.Y *= 6f;
                            Projectile.velocity.Y = velocity.Y * ((Vector2.Distance(player.Center + new Vector2(0f, -AerosmithHoverHeightOffset - 16), Projectile.Center)) / (12 * 16f));
                        }
                    }
                    else if (Projectile.Distance(player.Center) > 16 * 16f)
                    {
                        Projectile.tileCollide = false;
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= MaxFlightSpeed + player.moveSpeed;
                        Projectile.netUpdate = true;
                    }
                    else
                    {
                        Projectile.tileCollide = false;
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= player.moveSpeed + 6f;
                        Projectile.netUpdate = true;
                    }
                }
                else
                {
                    if (Projectile.Distance(target.Center) > 3 * 16f)
                    {
                        Projectile.velocity = target.position - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= MaxFlightSpeed;

                        Projectile.direction = 1;
                        if (Projectile.velocity.X < 0f)
                            Projectile.direction = -1;
                        Projectile.spriteDirection = Projectile.direction;
                        Projectile.netUpdate = true;
                    }

                    if (Projectile.Distance(target.Center) <= AutoModeDetectionDistance - (4 * 16f))
                    {
                        Projectile.velocity *= 0.86f;
                        if (shootCount <= 0 && Main.myPlayer == Projectile.owner)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = target.Center - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StandBullet>(), newProjectileDamage, 3f, Projectile.owner);
                            Main.projectile[projIndex].netUpdate = true;
                            SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                        }
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(bombless);
            writer.Write(remoteMode);
            writer.Write((byte)leftClickCooldown);
            writer.Write((short)(Projectile.velocity.X * 100));
            writer.Write((short)(Projectile.velocity.Y * 100));
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            bombless = reader.ReadBoolean();
            remoteMode = reader.ReadBoolean();
            leftClickCooldown = reader.ReadByte();
            Projectile.velocity.X = reader.ReadInt16() / 100f;
            Projectile.velocity.Y = reader.ReadInt16() / 100f;
        }

        public void SelectFrame()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
                if (bombless)
                {
                    if (Projectile.frame >= 2)
                        Projectile.frame = 0;
                }
                else
                {
                    if (Projectile.frame >= 4)
                        Projectile.frame = 2;
                }
            }
        }

        public override DrawData BuildStandDesummonDrawData()
        {
            standTexture = (Texture2D)ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad);
            int frameHeight = standTexture.Height / Main.projFrames[Projectile.type];
            Vector2 drawOffset = StandOffset;
            drawOffset.X *= Projectile.spriteDirection;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + drawOffset;
            Rectangle animRect = new Rectangle(0, frameHeight * Projectile.frame, standTexture.Width, frameHeight);
            Vector2 standOrigin = new Vector2(standTexture.Width / 2f, frameHeight / 2f);
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                effects = SpriteEffects.FlipHorizontally;

            return new DrawData(standTexture, drawPosition, animRect, Color.White, Projectile.rotation, standOrigin, 1f, effects, 0);
        }
    }
}