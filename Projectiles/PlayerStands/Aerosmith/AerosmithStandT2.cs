using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Aerosmith
{
    public class AerosmithStandT2 : StandClass
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/PlayerStands/Aerosmith/Aerosmith"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 40;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.netImportant = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 0;
            Projectile.ignoreWater = true;
        }

        public override float ProjectileSpeed => 12f;
        public override int ProjectileDamage => 42;
        public override int ShootTime => 12;      //+2 every tier
        public override StandAttackType StandType => StandAttackType.Ranged;
        public override string PoseSoundName => "VolareVia";
        public override string SpawnSoundName => "Aerosmith";
        public override bool CanUseRangeIndicators => false;

        private bool bombless = false;
        private bool fallingFromSpace = false;
        private bool remoteMode = false;
        private int leftMouse = 0;
        private int rightMouse = 0;
        private int accelerationTimer = 0;
        private SoundEffectInstance aerosmithWhirrSound;
        private const int AccelerationTime = 2 * 60;
        private const float MaxFlightSpeed = 8f;
        private const float WhirrSoundDistance = 86 * 16f;
        private const float AerosmithHoverHeightOffset = 3.5f * 16f;


        public override void OnSpawn(IEntitySource source)
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
            if (remoteMode)
                mPlayer.standControlStyle = MyPlayer.StandControlStyle.Remote;
            newProjectileDamage = (int)(newProjectileDamage * MathHelper.Clamp(1f - (Projectile.Distance(player.Center) / (350f * 16f)), 0.5f, 1f));

            fallingFromSpace = Projectile.position.Y < (Main.worldSurface * 0.35) * 16f;
            if (fallingFromSpace)
            {
                Projectile.frameCounter = 0;
                Projectile.velocity.Y += 0.3f;
                Projectile.netUpdate = true;
            }
            Vector2 rota = Projectile.Center - new Vector2(mouseX, mouseY);
            Projectile.rotation = (-rota * Projectile.direction).ToRotation();
            bombless = player.HasBuff(ModContent.BuffType<AbilityCooldown>());
            Projectile.tileCollide = true;

            if (Projectile.velocity.X > 0.5f)
                Projectile.spriteDirection = 1;
            if (Projectile.velocity.X < -0.5f)
                Projectile.spriteDirection = -1;

            if (leftMouse > 0)
                leftMouse--;
            if (rightMouse > 0)
                rightMouse--;

            if (remoteMode)
            {
                player.aggro -= 1200;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.standRemoteModeCameraPosition = Projectile.Center - new Vector2(halfScreenWidth, halfScreenHeight);
            }
            if (Projectile.owner != Main.myPlayer)
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
                    leftMouse = 10;

                    float mouseDistance = Vector2.Distance(new Vector2(mouseX, mouseY), Projectile.Center);

                    Projectile.spriteDirection = Projectile.direction;
                    accelerationTimer += 2;
                    if (accelerationTimer >= AccelerationTime)
                        accelerationTimer = AccelerationTime;

                    if (mouseDistance > 40f)
                    {
                        Projectile.velocity = new Vector2(mouseX, mouseY) - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= MaxFlightSpeed + player.moveSpeed;
                        Projectile.velocity *= accelerationTimer / (float)AccelerationTime;
                    }
                    else
                    {
                        Projectile.velocity = new Vector2(mouseX, mouseY) - Projectile.Center;
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
                if (Main.mouseRight && Projectile.owner == Main.myPlayer)
                {
                    rightMouse = 10;

                    if (mouseX > Projectile.Center.X)
                        Projectile.spriteDirection = 1;
                    if (mouseX < Projectile.Center.X)
                        Projectile.spriteDirection = -1;
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        Vector2 shootVel = new Vector2(mouseX, mouseY) - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 32f;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StandBullet>(), newProjectileDamage, 3f, Projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                        SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                    }
                }
                if (!remoteMode && leftMouse == 0)
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
                if (SpecialKeyPressedNoCooldown() && Projectile.owner == Main.myPlayer)
                {
                    remoteMode = !remoteMode;
                    if (remoteMode)
                        Main.NewText("Remote Mode: Active");
                    else
                        Main.NewText("Remote Mode: Disabled");
                }
                if (SecondSpecialKeyPressedNoCooldown() && !bombless)
                {
                    shootCount += newShootTime;
                    Projectile.frame = 2;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<AerosmithBomb>(), 0, 3f, Projectile.owner, 568 * (float)mPlayer.standDamageBoosts);
                    Main.projectile[proj].netUpdate = true;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(5));
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                remoteMode = false;
                Projectile.rotation = (Projectile.velocity * Projectile.direction).ToRotation();
                NPC target = FindNearestTarget(350f);
                if (target == null)
                {
                    if (Projectile.Distance(player.Center) < 60f)
                    {
                        if (Projectile.position.X >= player.position.X + 50f || WorldGen.SolidTile((int)(Projectile.position.X / 16) - 3, (int)(Projectile.position.Y / 16f) + 1))
                        {
                            Projectile.velocity.X = -2f;
                            Projectile.spriteDirection = Projectile.direction = -1;
                            Projectile.netUpdate = true;
                        }
                        if (Projectile.position.X < player.position.X - 50f || WorldGen.SolidTile((int)(Projectile.position.X / 16) + 3, (int)(Projectile.position.Y / 16f) + 1))
                        {
                            Projectile.velocity.X = 2f;
                            Projectile.spriteDirection = Projectile.direction = 1;
                            Projectile.netUpdate = true;
                        }
                        if (Projectile.position.Y > player.position.Y + 2f)
                        {
                            Projectile.velocity.Y = -2f;
                        }
                        if (Projectile.position.Y < player.position.Y - 2f)
                        {
                            Projectile.velocity.Y = 2f;
                        }
                        if (Projectile.position.Y < player.position.Y + 2f && Projectile.position.Y > player.position.Y - 2f)
                        {
                            Projectile.velocity.Y = 0f;
                            Projectile.netUpdate = true;
                        }
                    }
                    if (Projectile.Distance(player.Center) > 80f)
                    {
                        Projectile.tileCollide = false;
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 8f + player.moveSpeed;
                        Projectile.netUpdate = true;
                    }
                    if (Projectile.Distance(player.Center) >= 60f && Projectile.Distance(player.Center) <= 80f)
                    {
                        Projectile.tileCollide = false;
                        Projectile.velocity = player.Center - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= ((Projectile.Distance(player.Center) - 55f) * (8f + player.moveSpeed)) / 20;
                        Projectile.netUpdate = true;
                    }
                }
                if (target != null)
                {
                    if (Projectile.Distance(target.Center) > 45f)
                    {
                        Projectile.velocity = target.position - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 8f;

                        Projectile.direction = 1;
                        if (Projectile.velocity.X < 0f)
                            Projectile.direction = -1;

                        Projectile.spriteDirection = Projectile.direction;
                        Projectile.netUpdate = true;
                    }
                    if (shootCount <= 0)
                    {
                        if (Main.myPlayer == Projectile.owner)
                        {
                            shootCount += newShootTime;
                            SoundEngine.PlaySound(SoundID.Item11, Projectile.position);
                            Vector2 shootVel = target.Center - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StandBullet>(), newProjectileDamage, 3f, Projectile.owner);
                            Main.projectile[proj].netUpdate = true;
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
            writer.Write(leftMouse);
            writer.Write(rightMouse);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            bombless = reader.ReadBoolean();
            remoteMode = reader.ReadBoolean();
            leftMouse = reader.ReadInt32();
            rightMouse = reader.ReadInt32();
        }

        public void SelectFrame()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame += 1;
                Projectile.frameCounter = 0;
                if (!bombless)
                {
                    if (Projectile.frame >= 4)
                        Projectile.frame = 2;
                }
                else
                {
                    if (Projectile.frame >= 2)
                        Projectile.frame = 0;
                }
            }
        }
    }
}